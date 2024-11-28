<template>
  <div class="container py-4 mt-5">
    <form @submit.prevent="submitForm">
      <div v-if="step === 1">
        <p class="lead text-center">Select service</p>
        <!-- Step 1: Booking Service -->
        <div class="d-flex flex-column gap-3">
          <div v-for="service in services"
               :key="service.id"
               class="service-card bg-light-subtle p-2 rounded-2 shadow-sm btn text-dark text-start"
               @click="selectService(service)">
            <div class="card-header">
              <p class="lead m-0 fw-bold">{{ service.name }}</p>
              <span class="price">${{ service.price }}</span>
            </div>
            <p class="text-dark">{{ service.description }}</p>
            <div class="text-end">
              <i class="bi bi-arrow-right-circle"></i>
            </div>
          </div>
        </div>
      </div>

      <div v-if="step === 2">
        <p class="lead text-center">Select date and time</p>
        <!-- Step 2: Date and time -->
        <div class="datepicker-container m-2">
          <div class="calendar rounded-2 shadow-sm">
            <div class="calendar-header">
              <button @click="prevMonth" class="nav-button" type="button">‹</button>
              <span>{{ monthNames[currentMonth] }} {{ currentYear }}</span>
              <button @click="nextMonth" class="nav-button" type="button">›</button>
            </div>
            <div class="calendar-body">
              <div class="calendar-weekdays">
                <span v-for="day in weekdays" :key="day" class="weekday">{{ day }}</span>
              </div>
              <div class="calendar-dates">
                <span v-for="(date, index) in getDates()"
                      :key="index"
                      :class="['date', { 'current-date': isToday(date), 'selected-date': isSelectedDate(date) }]"
                      @click="selectDate(date)">
                  {{ date.getDate() }}
                </span>
              </div>
            </div>
          </div>
        </div>
        <!--TIME PICKER-->
        <div class="timepicker-container m-2">
          <div class="timepicker rounded-2 shadow-sm">
            <div class="timepicker-body p-2">
              <div class="timepicker-section mb-2">
                <label class="form-label fw-bold">Hour</label>
                <select v-model="selectedHour" class="timepicker-select">
                  <option v-for="hour in hours" :key="hour" :value="hour">{{ hour }}</option>
                </select>
              </div>
              <div class="timepicker-section">
                <label class="form-label fw-bold">Minute</label>
                <select v-model="selectedMinute" class="timepicker-select">
                  <option v-for="minute in minutes" :key="minute" :value="minute">{{ minute }}</option>
                </select>
              </div>
              <div class="timepicker-section">
                <label class="form-label fw-bold">AM/PM</label>
                <select v-model="selectedPeriod" class="timepicker-select">
                  <option value="AM">AM</option>
                  <option value="PM">PM</option>
                </select>
              </div>
            </div>
          </div>
        </div>

        <div class="d-flex justify-content-between">
          <button type="button"
                  class="btn w-25"
                  @click="goToStep(1)">
            <i class="bi bi-arrow-left-circle"></i>
          </button>
          <button type="button"
                  class="btn w-25"
                  @click="goToStep(3)">
            <i class="bi bi-arrow-right-circle"></i>
          </button>
        </div>
      </div>
      <div v-if="step === 3">
        <p class="lead text-center">Enter personal details</p>
        <!-- Step 3: Personal Details -->
        <div v-if="formData.service !== ''">
          <p class="m-0 bg-success-subtle p-1 mb-3 rounded-1 shadow-sm p-2">
            You are booking: {{ formData.service }} on {{ formattedDate }} at {{ formattedTime }}
          </p>
        </div>
        <div class="mb-2">
          <label for="name" class="form-label">Name</label>
          <input type="text"
                 id="name"
                 class="form-control"
                 v-model="formData.name"
                 placeholder=""
                 required />
        </div>

        <div class="mb-2">
          <label for="email" class="form-label">Email</label>
          <input type="email"
                 id="email"
                 class="form-control"
                 v-model="formData.email"
                 placeholder=""
                 required />
        </div>

        <div class="mb-2">
          <label for="phone" class="form-label">Phone Number</label>
          <input type="tel"
                 id="phone"
                 class="form-control"
                 v-model="formData.phone"
                 placeholder=""
                 required />
        </div>

        <div class="mb-2">
          <label for="street" class="form-label">Street Address</label>
          <input type="text"
                 id="street"
                 class="form-control"
                 v-model="formData.address.street"
                 placeholder=""
                 required />
        </div>

        <div class="row mb-2">
          <div class="col">
            <label for="city" class="form-label">City</label>
            <input type="text"
                   id="city"
                   class="form-control"
                   v-model="formData.address.city"
                   placeholder=""
                   required />
          </div>
          <div class="col">
            <label for="state" class="form-label">State</label>
            <input type="text"
                   id="state"
                   class="form-control"
                   v-model="formData.address.state"
                   placeholder=""
                   required />
          </div>
          <div class="col">
            <label for="zip" class="form-label">ZIP Code</label>
            <input type="text"
                   id="zip"
                   class="form-control"
                   v-model="formData.address.zip"
                   placeholder=""
                   required />
          </div>
        </div>
        <div class="d-flex justify-content-between">
          <button type="button"
                  class="btn w-25"
                  @click="goToStep(2)">
            <i class="bi bi-arrow-left-circle"></i>
          </button>
          <button type="submit" class="btn w-25">
            <i class="bi bi-send-check-fill"></i>
          </button>
        </div>
      </div>
    </form>
  </div>
</template>

<script>
  import moment from 'moment';

  export default {
    name: "BookingForm",
    //components: {
    //  DatePicker,
    //},
    data() {
      const today = new Date();
      const currentTime = new Date();
      return {
        currentYear: today.getFullYear(),
        currentMonth: today.getMonth(),
        selectedDate: today,
        weekdays: ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'],
        monthNames: [
          'January', 'February', 'March', 'April', 'May', 'June',
          'July', 'August', 'September', 'October', 'November', 'December'
        ],
        selectedHour: String(currentTime.getHours() % 12 || 12).padStart(2, '0'), // Convert to 12-hour format
        selectedMinute: String(currentTime.getMinutes()).padStart(2, '0'),
        selectedPeriod: currentTime.getHours() >= 12 ? 'PM' : 'AM', // Determine AM or PM
        step: 1, // Step tracking
        formData: {
          name: "",
          email: "",
          phone: "",
          address: {
            street: "",
            city: "",
            state: "",
            zip: "",
          },
          service: "",
          date: "",
          time: "",
        },
        services: [
          {
            id: 1,
            name: "Spa Package",
            price: 150,
            description: "Relax with a full-body massage and facial treatment.",
          },
          {
            id: 2,
            name: "Yoga Session",
            price: 50,
            description: "A rejuvenating 1-hour guided yoga session.",
          },
          {
            id: 3,
            name: "Gourmet Dinner",
            price: 120,
            description: "A 3-course fine dining experience with wine pairing.",
          },
          {
            id: 4,
            name: "Private Tour",
            price: 200,
            description: "Explore the city with a personalized private tour guide.",
          },
        ],
      };
    },
    computed: {
      hours() {
        // 1–12 for 12-hour format
        return Array.from({ length: 12 }, (_, i) => String(i + 1).padStart(2, '0'));
      },
      minutes() {
        // 0–59 for minutes
        return Array.from({ length: 60 }, (_, i) => String(i).padStart(2, '0'));
      },
      formattedTime() {
        // Format as HH:MM AM/PM
        return `${this.selectedHour}:${this.selectedMinute} ${this.selectedPeriod}`;
      },
      formattedDate() {
        return moment(this.selectedDate).format('YYYY/MM/DD dddd');
      }
    },
    methods: {
      selectService(service) {
        this.formData.service = service.name; // Set the service name in formData
        this.goToStep(2); // Navigate to step 2
      },
      goToStep(stepNumber) {
        this.step = stepNumber;
      },
      submitForm() {
        // Handle form submission (e.g., send to an API)
        alert(`Booking submitted: \n${JSON.stringify(this.formData, null, 2)}`);

        // Clear the form
        this.step = 1;
        this.formData = {
          name: "",
          email: "",
          phone: "",
          address: {
            street: "",
            city: "",
            state: "",
            zip: "",
          },
          service: "",
          date: "",
          time: "",
        };
      },
      getDates() {
        const dates = [];
        const firstDayOfMonth = new Date(this.currentYear, this.currentMonth, 1);
        const lastDayOfMonth = new Date(this.currentYear, this.currentMonth + 1, 0);

        // Add days from the previous month to fill the first week
        const startDay = firstDayOfMonth.getDay();
        for (let i = 0; i < startDay; i++) {
          const prevDate = new Date(this.currentYear, this.currentMonth, -i);
          dates.unshift(prevDate);
        }

        // Add all dates from the current month
        for (let i = 1; i <= lastDayOfMonth.getDate(); i++) {
          dates.push(new Date(this.currentYear, this.currentMonth, i));
        }

        // Add days from the next month to complete the grid
        while (dates.length % 7 !== 0) {
          const nextDate = new Date(this.currentYear, this.currentMonth + 1, dates.length % 7);
          dates.push(nextDate);
        }

        return dates;
      },
      prevMonth() {
        this.currentMonth -= 1;
        if (this.currentMonth < 0) {
          this.currentMonth = 11;
          this.currentYear -= 1;
        }
      },
      nextMonth() {
        this.currentMonth += 1;
        if (this.currentMonth > 11) {
          this.currentMonth = 0;
          this.currentYear += 1;
        }
      },
      selectDate(date) {
        this.selectedDate = date;
      },
      isToday(date) {
        const today = new Date();
        return (
          date.getFullYear() === today.getFullYear() &&
          date.getMonth() === today.getMonth() &&
          date.getDate() === today.getDate()
        );
      },
      isSelectedDate(date) {
        return (
          this.selectedDate.getFullYear() === date.getFullYear() &&
          this.selectedDate.getMonth() === date.getMonth() &&
          this.selectedDate.getDate() === date.getDate()
        );
      },
    },
  };
</script>

<style scoped>
  .service-card {
    border-bottom: 4px solid midnightblue;
  }

  .form-label {
    font-size: 0.8rem !important;
  }

  .form-select {
    font-size: 0.8rem !important;
  }

  .container {
    max-width: 600px;
    margin: 0 auto;
    background-color: #f9f9f9;
    padding: 20px;
    border-radius: 8px;
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
  }

  h2 {
    font-size: 1.8rem;
    color: #333;
  }

  button:hover {
    background-color: #0056b3;
    border-radius: 5px;
  }

  .text-muted {
    font-size: 0.9rem;
    font-style: italic;
  }

  .datepicker-container {
    max-width: 300px;
    margin: 0 auto;
    font-family: Arial, sans-serif;
  }

  .calendar {
    padding: 10px;
    background-color: #fff;
  }

  .calendar-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 10px;
    font-weight: bold;
  }

  .nav-button {
    border: none;
    background: none;
    cursor: pointer;
    font-size: 16px;
    padding: 5px;
  }

  .calendar-weekdays,
  .calendar-dates {
    display: grid;
    grid-template-columns: repeat(7, 1fr);
    text-align: center;
    gap: 5px;
  }

  .weekday {
    font-weight: bold;
  }

  .date {
    cursor: pointer;
    padding: 5px;
    border-radius: 50%;
    transition: background-color 0.3s ease;
  }

    .date:hover {
      background-color: #007BFF;
    }

  .current-date {
    background-color: #007BFF;
    color: #fff;
  }

  .selected-date {
    background-color: #28a745;
    color: #fff;
  }
  /*TIME PICKER CSS*/
  .timepicker {
    display: flex;
    flex-direction: column;
    padding: 1rem;
    background: #fff;
    font-size: 0.8rem;
  }

  .timepicker-body {
    display: flex;
    justify-content: space-around;
  }

  .timepicker-section {
    display: flex;
    flex-direction: column;
    align-items: center;
  }

  .timepicker-select {
    padding: 0.2rem;
    border: 1px solid #ccc;
    border-radius: 4px;
    font-size: 0.8rem;
    color: #000000;
  }
</style>


