<template>
  <div class="container py-2 mb-5 text-white min-vh-100">
    <!-- Services Section -->
    <div class="mt-3 mb-5">
      <!-- Button to Fetch services -->
      <!--<div class="text-end">
        <button @click="fetchServices"
                class="btn btn-small rounded-pill py-2 fw-bold"
                style="background-color: #f8b195; font-size: 0.8rem;">
          <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" fill="#ffffff" class="bi bi-arrow-clockwise" viewBox="0 0 16 16">
            <path fill-rule="evenodd" d="M8 3a5 5 0 1 0 4.546 2.914.5.5 0 0 1 .908-.417A6 6 0 1 1 8 2z" />
            <path d="M8 4.466V.534a.25.25 0 0 1 .41-.192l2.36 1.966c.12.1.12.284 0 .384L8.41 4.658A.25.25 0 0 1 8 4.466" />
          </svg>
        </button>
      </div>-->
      <!-- Loading State with Placeholder Cards -->
      <div v-if="loading" class="mt-4">
        <div class="skeleton-card mb-3" v-for="n in 6" :key="n">
          <div class="skeleton-title"></div>
          <div class="skeleton-line"></div>
          <div class="skeleton-line"></div>
          <div class="skeleton-line short"></div>
        </div>
      </div>

      <div style="margin-top: 100px;" v-if="services !== null">
        <!-- Month and Year Section -->
        <div class="row justify-content-center align-items-center m-0 p-0 rounded-bottom-2 fixed-top" style="background-color: #001524;">
          <!--<div class="col-auto">
            <i class="bi bi-caret-left-fill" @click="previousMonth"></i>
          </div>-->
          <!--<div class="col text-center">
            <h4>{{ currentMonth }}</h4>
          </div>-->
          <!--<div class="col-auto">
            <i class="bi bi-caret-right-fill" @click="nextMonth"></i>
          </div>-->
          <!-- Week Range Section -->
          <div class="row align-items-center text-center m-1">
            <div class="col-auto">
              <i class="bi bi-chevron-left px-2 py-1 fs-6 rounded-5 orange-bg" @click="previousWeek"></i>
            </div>
            <div class="col text-center">
              <p class="p-1 rounded-5 m-0 fs-5 rounded-5 fs-6 orange-bg">{{ weekRange }}, <span class="fw-bold">{{ currentMonth }}</span></p>
            </div>
            <div class="col-auto">
              <i class="bi bi-chevron-right px-2 py-1 fs-6 rounded-5 orange-bg" @click="nextWeek"></i>
            </div>
          </div>
        </div>

        <!--Services Section-->
        <div v-for="record in services">
          <p v-if="isDateInWeekRange(record.serviceDate)" :id="isToday(record.serviceDate) ? 'today' : null"  :class="{
              'mb-1': true,
              'fs-5': true,
              'today-class': isToday(record.serviceDate)
            }">
            {{ formatMonthAndDay(record.serviceDate) }}
          </p>
          <div>
            <div class="row d-flex justify-content-between" v-if="isDateInWeekRange(record.serviceDate)" v-for="service in record.services">
              <div class="col-3 mb-2">
                <p class="mb-1">{{ service.time }}</p>
              </div>
              <div class="col-8">
                <div class="m-0 details-card mb-3 rounded-3">
                  <p class="mb-0"><span class="fw-bold">Client: </span>{{ service.name }}</p>
                  <p class="mb-0"><span class="fw-bold">Service requested: </span>{{ service.serviceName }}</p>
                  <!--Collapse Button-->
                  <button class="btn btn-sm w-100 btn-details mt-2 mb-2" type="button"
                          :data-bs-toggle="'collapse'" :data-bs-target="'#collapse' + service.serviceId"
                          :aria-expanded="false" :aria-controls="'collapse' + service.serviceId">
                    <i class="bi bi-chevron-double-down"></i> Contact Info
                  </button>
                  <!--Collapsible Section-->
                  <div :id="'collapse' + service.serviceId" class="collapse">
                    <p class="card-text mb-1">
                      <strong>
                        Email:
                      </strong>
                      <a :href="'mailto:' + service.email" class="text-decoration-none text-white-50">
                        {{ service.email }}
                        <i class="bi bi-envelope-plus"></i>
                      </a>
                    </p>
                    <p class="card-text mb-1">
                      <strong>
                        Phone:
                      </strong>
                      <a :href="'tel:' + service.phone" class="text-decoration-none text-white-50">
                        {{ service.phone }}
                        <i class="bi bi-telephone-outbound"></i>
                      </a>
                    </p>
                    <p class="card-text">
                      <div v-if="!service.address">
                        Online
                      </div>
                      <div v-else>
                        <strong>Address: <span>{{ service.address }}</span></strong>
                      </div>
                    </p>
                  </div>
                  <div class="text-end mt-2">
                    <i @click="deleteService(service.serviceId)" class="bi bi-trash3 me-2"> Cancel</i>
                    <i @click="test" class="bi bi-pencil"> Edit</i>
                  </div>
                </div>
              </div>
            </div>
          </div>
          <hr v-if="isDateInWeekRange(record.serviceDate)" class="border-2 mt-3 w-75 m-4">
        </div>
      </div>
    </div>
  </div>
</template>

<script>
  import axios from 'axios';
  import moment from 'moment';

  export default {
    data() {
      return {
        today: moment().format('MMM DD'),
        todaysDate: "",
        currentMonth: moment().format('MMM YYYY'),
        weekDays: moment.weekdays(),
        weekRange: "",
        loading: false,
        services: null,
        serviceId: null,
        isPrd: true,
        startOfWeek: moment().startOf('week').format('YYYY-MM-DD'),
        endOfWeek: moment().endOf('week').format('YYYY-MM-DD'),
        GetservicesUrl: "",
        DeleteservicesUrl: "",
      };
    },
    async created() {
      this.getWeekRange();
      this.GetservicesUrl = this.isPrd ? "https://engfuel.com/Bookings/GetAllBookings?businessId=YohanJS" : "https://localhost:7144/Bookings/GetAllBookings?businessId=YohanJS";
      this.DeleteservicesUrl = this.isPrd ? "https://engfuel.com/Bookings/DeleteBooking" : "https://localhost:7144/Bookings/DeleteBooking";
      await this.fetchServices();
    },
    watch: {
      weekRange(newVal, oldVal) {
        this.getWeekRange();
      },
      startOfWeek() {
        this.updateDisplayedServices();
      },
      endOfWeek() {
        this.updateDisplayedServices();
      },
      currentMonth() {
        this.updateDisplayedServices();
      }
    },
    methods: {
      formatMonthAndDay(serviceDate) {
        const today = moment().startOf('day');
        const serviceDateMoment = moment(serviceDate, "YYYY/MM/DD", true);

        if (serviceDateMoment.isValid() && serviceDateMoment.isSame(today, 'day')) {
          return serviceDateMoment.format("MMM D") + " Today";
        }

        return serviceDateMoment.isValid() ? serviceDateMoment.format("MMM D") : "Invalid Date";
      },
      isToday(serviceDate) {
        const today = moment().startOf('day'); // Normalize today to midnight
        const date = moment(serviceDate, "YYYY/MM/DD", true); // Strict parsing of serviceDate
        return date.isValid() && date.isSame(today, 'day');
      },
      isTodayBooking() {
        const today = moment(); // Get today's date
        const endOfWeek = moment().endOf('week'); // Get the end of the current week
        const days = [];

        // Loop from today to the end of the week
        while (today.isSameOrBefore(endOfWeek, 'day')) {
          days.push(today.format('dddd')); // Add day of the week (e.g., Monday, Tuesday) to the list
          today.add(1, 'day'); // Move to the next day
        }

        for (var i = 0; i < days.length; i++) {
          if (days[i] === moment().format('dddd')) {
            return moment().format('MMM YY') + " Today";
          }
          return day
        }
      },
      updateDisplayedServices() {
        // Force re-evaluation of services by simply triggering Vue's reactivity
        //TODO read more about this*************************
        this.services = [...this.services]; // Shallow copy to trigger re-render
      },
      getDay(date) {
        return moment(new Date(date)).format('dddd');
      },
      previousMonth() {
        this.currentMonth = moment(this.currentMonth, 'MMM YYYY').subtract(1, 'months').startOf('month').format('MMM YYYY');
      },
      nextMonth() {
        this.currentMonth = moment(this.currentMonth, 'MMM YYYY').add(1, 'months').startOf('month').format('MMM YYYY');
      },
      getWeekRange() {
        const startOfWeekFormatted = moment(this.startOfWeek, 'YYYY-MM-DD').format('DD');
        const endOfWeekFormatted = moment(this.endOfWeek, 'YYYY-MM-DD').format('DD');

        this.weekRange = `Week ${startOfWeekFormatted}-${endOfWeekFormatted}`;
      },
      isDateInWeekRange(serviceDate) {
        const currentStartDate = moment(this.startOfWeek, 'YYYY-MM-DD');
        const currentEndDate = currentStartDate.clone().endOf('week');
        const serviceMoment = moment(serviceDate, 'YYYY-MM-DD');

        return serviceMoment.isBetween(currentStartDate, currentEndDate, null, '[]');
      },
      isbookingBookedInWeek(booking) {
        if (booking.services.length > 0) {
          for (let i = 0; i < booking.services.length; i++) {
            if (this.isDateInWeekRange(booking.services[i].date)) {
              return true;
            }
          }
        }
        return false;
      },
      nextWeek() {
        const newStartOfWeek = moment(this.startOfWeek, 'YYYY-MM-DD').add(7, 'days'); // Parse and add 7 days

        this.startOfWeek = moment(newStartOfWeek).startOf('week').format('YYYY-MM-DD');
        this.currentMonth = newStartOfWeek.format('MMM YYYY');

        const newEndOfWeek = newStartOfWeek.clone().endOf('week');

        this.endOfWeek = newEndOfWeek.format('YYYY-MM-DD');
        this.getWeekRange();
      },
      previousWeek() {
        const newStartOfWeek = moment(this.startOfWeek, 'YYYY-MM-DD').subtract(7, 'days'); // Parse and subtract 7 days

        this.startOfWeek = moment(newStartOfWeek).startOf('week').format('YYYY-MM-DD');
        this.currentMonth = newStartOfWeek.format('MMM YYYY');

        const newEndOfWeek = newStartOfWeek.clone().endOf('week');

        this.endOfWeek = newEndOfWeek.format('YYYY-MM-DD');
        this.getWeekRange();
      },
      async fetchServices() {
        this.services = null;
        this.loading = true;
        try {
          const response = await axios.get(this.GetservicesUrl);
          this.services = response.data;

          // Wait for DOM updates
          //this.$nextTick() ensures that any DOM updates triggered by changes to this.services are completed before proceeding.
          await this.$nextTick();
          this.todaysDate = document.getElementById("today");

          if (this.todaysDate) {
            this.todaysDate.scrollIntoView({ behavior: "smooth", block: "start" });
          }
        } catch (error) {
          alert("Error fetching services:", error);
        } finally {
          this.loading = false;
        }
      },
      async deleteService(id) {
        try {
          const response = await axios.post(this.DeleteservicesUrl,
            {
              serviceId: id,
            }
          );
        } catch (error) {
          alert("Error deleting services:", error);
        } finally {
          this.loading = false;
          this.fetchServices();
        }
      },
    },
  };
</script>

<style scoped>
  .container {
    background-color: #001524;
  }
  #today {
    top: 10px;
    /* Without this, the element will be aligned to the top of the page */
    scroll-margin-top: 1000px;
  }
  /* Loading Skeleton */
  .skeleton-card {
    background-color: #e0e0e0;
    padding: 1rem;
    border-radius: 8px;
    margin-bottom: 1rem;
    animation: pulse 1.5s infinite ease-in-out;
  }

  .skeleton-title {
    height: 20px;
    background-color: #cfcfcf;
    width: 50%;
    margin-bottom: 0.5rem;
    border-radius: 4px;
  }

  .skeleton-line {
    height: 15px;
    background-color: #d9d9d9;
    margin-bottom: 0.5rem;
    border-radius: 4px;
  }

  .skeleton-line.short {
    width: 70%;
  }

  @keyframes pulse {
    0% {
      background-color: #e0e0e0;
    }

    50% {
      background-color: #f0f0f0;
    }

    100% {
      background-color: #e0e0e0;
    }
  }

  .orange-bg {
    background-color: #F28C28;
  }

  .today-class {
    color: #F28C28;
  }
  .orange-txt {
    color: #F28C28;
  }

  .btn-details {
    background-color: #F28C28;
    color: #ffffff;
    border-radius: 5px;
    transition: background-color 0.3s ease;
  }

  .btn-details:active {
    color: #192c39;
  }

  .details-card {
    font-size: 0.8rem;
    background-color: #003357;
    padding: 10px;
    box-shadow: 2px 2px 5px rgba(200, 200, 200, 0.3);
  }
  /* Modern pastel card styling */
  .pastel-card {
    background-color: #192c39; /* Soft gradient */
    border-radius: 16px;
    border: none;
    box-shadow: 0 6px 15px rgba(200, 200, 200, 0.2);
    transition: transform 0.3s ease, box-shadow 0.3s ease;
    display: flex;
    flex-direction: column;
    height: 100%; /* Ensure cards take full height */
  }

  .pastel-card:hover {
    transform: translateY(-8px);
    box-shadow: 0 12px 25px rgba(200, 200, 200, 0.3);
  }

  /* Card body styling */
  .pastel-card .card-body {
    padding: 1rem;
    font-size: 0.8rem;
    background-color: #e0e0e0;
    flex: 1; /* Make the body fill available space */
    display: flex;
    flex-direction: column;
  }

  /* Card title styling */
  .pastel-card .card-title {
    font-size: 0.9rem; /* Slightly smaller font for mobile look */
    margin-bottom: 1rem;
  }

  /* Card text styling */
  .pastel-card .card-text {
    font-size: 0.8rem; /* Smaller font for a modern mobile look */
    margin-bottom: 0.75rem;
  }

  /* List group styling */
  .pastel-list-item {
    background-color: #f2f4ff; /* Light pastel blue */
    border: none;
    border-radius: 8px;
    margin-bottom: 0.75rem;
    padding: 0.75rem;
    font-size: 0.9rem; /* Smaller font for consistency */
    transition: background-color 0.3s ease;
  }

  .pastel-list-item:hover {
    background-color: #e5e7ff; /* Slightly darker pastel blue on hover */
  }

  /* Centered grid layout */
  .justify-content-center {
    display: flex;
    flex-wrap: wrap;
    justify-content: center;
  }

  .mr {
    margin-right: 10px;
  }
</style>
