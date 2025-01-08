<!--Template-->
<template>
  <div id="bookingFormContainer" class="container py-2 p-2 mb-5 text-dark min-vh-100">
    <div class="text-center mb-4 mt-2">
      <div class="px-5" role="group" aria-label="First group">
        <button @click="goToStep(1)" id="step1" type="button" class="btn btn-primary m-1 rounded-5 my-bg border-0 small-btn fw-bold steel-blue-color">1</button>
        <button @click="goToStep(2)" id="step2" type="button" class="btn btn-primary m-1 rounded-5 my-bg border-0 small-btn">2</button>
        <button @click="goToStep(3)" id="step3" type="button" class="btn btn-primary m-1 rounded-5 my-bg border-0 small-btn">3</button>
        <button @click="goToStep(4)" id="step4" type="button" class="btn btn-primary m-1 rounded-5 my-bg border-0 small-btn">4</button>
        <button @click="goToStep(5)" id="step5" type="button" class="btn btn-primary m-1 rounded-5 my-bg border-0 small-btn">5</button>
      </div>
    </div>

    <form @submit.prevent="submitForm" class="box-shadow p-2 rounded-1 min-vh-100">
      <!--Booking Details-->
      <!-- Booking Details -->
      <div v-if="step === 5 && formData.service !== ''" class="text-white p-4 m-3 rounded box-shadow">
        <p class="mb-3 text-center fs-6 fw-bold">Your Appointment Details:</p>
        <div class="mb-2">
          <p class="mb-1"><strong>Meeting Type:</strong> {{ formData.meetingType }}</p>
          <p class="mb-1"><strong>Service:</strong> {{ formData.service }}</p>
          <p class="mb-1"><strong>Date:</strong> {{ formattedDate }}</p>
          <p class="mb-1"><strong>Time:</strong> {{ formattedTime }}</p>
        </div>
      </div>

      <!-- Step 1: Booking Service -->
      <div v-if="step === 1">
        <p class="text-center mb-1 mt-5 pt-2 text-white">Select service</p>
        <div class="d-flex flex-column gap-3 mb-5">
          <div v-for="service in services"
               :key="service.id"
               class="service-card border-0 rounded-2 text-white m-3 btn"
               @click="selectService(service)"
               :style="{ backgroundImage: `url(${service.image})` }">
            <div class="overlay rounded-2 d-flex flex-column">
              <h4 class="mb-1 p-3 card-header">{{ service.name }}</h4>
              <p class="mb-2" style="font-size: 0.8rem;">{{ service.description }}</p>
              <div class="text-end m-3">
                <i class="bi bi-arrow-right-circle orange-txt"></i>
              </div>
            </div>
          </div>
        </div>
      </div>

      <!-- Step 2 In Peson/Online/Phone-->
      <div v-if="step === 2" class="text-center text-white p-2 mt-5">
        <p class="text-center mb-3 mt-5 pt-2 text-white">Type of meeting</p>
        <button @click="inPersonMeeting(true)" class="btn btn-outline-primary border-0 w-50 mb-2 text-white box-shadow">
          <span class="f-s">
            In Person
            <i class="bi bi-arrow-right-circle orange-txt"></i>
          </span>
        </button>
        <br />
        <button @click="inPersonMeeting(false)" class="btn btn-outline-primary border-0 w-50 text-white box-shadow">
          <span class="f-s">
            Online/Phone
            <i class="bi bi-arrow-right-circle orange-txt"></i>
          </span>
        </button>
        <div class="d-flex justify-content-between rounded-2 shadow-sm mx-auto" style="max-width: 300px;">
          <button type="button"
                  class="btn w-25"
                  @click="goToStep(1)">
            <i class="bi bi-arrow-left-circle text-dark"></i>
          </button>
          <button type="button"
                  class="btn w-25"
                  @click="goToStep(3)">
            <i class="bi bi-arrow-right-circle text-dark"></i>
          </button>
        </div>
      </div>

      <!-- Step 3: Date -->
      <div v-if="step === 3">
        <p class="text-center mb-3 mt-5 pt-2 text-white">Select date</p>
        <div class="datepicker-container m-2 mx-auto">
          <div class="calendar rounded-2">
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

        <!--<div class="d-flex justify-content-between my-bg rounded-2 shadow-sm mx-auto" style="max-width: 300px;">
      <button type="button"
              class="btn w-25"
              @click="goToStep(2)">
        <i class="bi bi-arrow-left-circle text-dark"></i>
      </button>
      <button type="button"
              class="btn w-25"
              @click="goToStep(4)">
        <i class="bi bi-arrow-right-circle text-dark"></i>
      </button>
    </div>-->
      </div>

      <!-- Step 4: Time Picker -->
      <div v-if="step === 4">
        <p class="text-center mb-3 mt-5 pt-2 text-white">Select time</p>
        <div class="timepicker-container m-2 mx-auto rounded-2">
          <div class="time-grid">
            <div v-for="(time, index) in timeSlots"
                 :key="index"
                 :class="['time-box shadow-sm', { 'taken': takenTimes.includes(time), 'selected': selectedTime === time }]"
                 @click="!takenTimes.includes(time) && selectTime(time)">
              {{ time }}
            </div>
          </div>
        </div>
      </div>

      <!-- Step 5: Personal Details -->
      <div v-if="step === 5">
        <p class="text-center mb-3 mt-5 pt-2 text-white">Enter personal details</p>
        <!--In Person-->
        <div class="p-3 m-3 rounded-2 box-shadow">
          <!--In Person-->
          <div v-if="isInPerson">

            <div class="mb-2">
              <label for="name" class="form-label text-white">Name</label>
              <input type="text"
                     id="name"
                     class="form-control"
                     v-model="formData.name"
                     placeholder=""
                     required />
            </div>

            <div class="mb-2">
              <label for="email" class="form-label text-white">Email</label>
              <input type="email"
                     id="email"
                     class="form-control"
                     v-model="formData.email"
                     placeholder=""
                     required />
            </div>

            <div class="mb-2">
              <label for="phone" class="form-label text-white">Phone Number</label>
              <input type="tel"
                     id="phone"
                     class="form-control"
                     v-model="formData.phone"
                     placeholder=""
                     required />
            </div>

            <div class="mb-2">
              <label for="street" class="form-label text-white">Street Address</label>
              <input type="text"
                     id="street"
                     class="form-control"
                     v-model="formData.street"
                     placeholder=""
                     required />
            </div>

            <div class="row mb-2">
              <div class="col">
                <label for="city" class="form-label text-white">City</label>
                <input type="text"
                       id="city"
                       class="form-control"
                       v-model="formData.city"
                       placeholder=""
                       required />
              </div>
              <div class="col">
                <label for="state" class="form-label text-white">State</label>
                <input type="text"
                       id="state"
                       class="form-control"
                       v-model="formData.state"
                       placeholder=""
                       required />
              </div>
              <div class="col">
                <label for="zip" class="form-label text-white">ZIP Code</label>
                <input type="text"
                       id="zip"
                       class="form-control"
                       v-model="formData.zip"
                       placeholder=""
                       required />
              </div>
            </div>
          </div>
          <!--Online/Phone-->
          <div v-else>

            <div class="mb-2">
              <label for="name" class="form-label text-white">Name</label>
              <input type="text"
                     id="name"
                     class="form-control"
                     v-model="formData.name"
                     placeholder=""
                     required />
            </div>

            <div class="mb-2">
              <label for="email" class="form-label text-white">Email</label>
              <input type="email"
                     id="email"
                     class="form-control"
                     v-model="formData.email"
                     placeholder=""
                     required />
            </div>

            <div class="mb-2">
              <label for="phone" class="form-label text-white">Phone Number</label>
              <input type="tel"
                     id="phone"
                     class="form-control"
                     v-model="formData.phone"
                     placeholder=""
                     required />
            </div>
          </div>
          <!--Buttons Section-->
          <div class="d-flex justify-content-between mt-4">
            <button type="button"
                    class="btn"
                    @click="goToStep(4)">
              <i class="bi bi-arrow-left-circle orange-txt"></i>
            </button>
            <div v-if="displaySpinnerMessage">
              <button class="btn btn-outline-success border-0" type="button" disabled>
                <span class="spinner-border spinner-border-sm" aria-hidden="true"></span>

              </button>
            </div>
            <div v-else>
              <button type="submit" class="btn w-25">
                <i class="bi bi-send-check-fill orange-txt"></i>
              </button>
            </div>
          </div>
        </div>
      </div>
    </form>
    <!--SUCCESS MODAL-->
    <div>
      <div class="modal fade" id="submissionModal" tabindex="-1" aria-labelledby="submissionModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
          <div class="modal-content modern-modal rounded-2 border-0 shadow-lg">
            <div class="modal-header border-0">
              <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body text-center p-4">
              <h5 class="modal-title fw-bold mb-1">Thank You!</h5>
              <i class="bi bi-emoji-smile-fill"></i>
              <p>Your quote appointment has been successfully scheduled. We’ll be in touch soon!</p>
            </div>
          </div>
        </div>
      </div>
      <!--<div v-if="errorMessage != ''" class="alert alert-danger mt-2" role="alert">
        {{ errorMessage }}
      </div>-->
    </div>
  </div>
</template>

<!--JS-->
<script>
  import moment from 'moment';
  import axios from "axios";
  import { Modal } from "bootstrap";

  export default {
    name: "BookingForm",
    //components: {
    //  DatePicker,
    //},
    data() {
      const today = new Date();
      const currentTime = new Date();
      return {
        isPrd: true,
        CreateBookingUrl: "",
        displaySpinnerMessage: false,
        currentYear: today.getFullYear(),
        currentMonth: today.getMonth(),
        selectedDate: today,
        weekdays: Array.from({ length: 7 }, (_, index) => moment().weekday(index).format('ddd')),
        monthNames: Array.from({ length: 12 }, (_, index) => moment().month(index).format('MMMM')),
        timeSlots: this.generateTimeSlots(),
        selectedTime: null,
        takenTimes: [],
        step: 1,
        isInPerson: false,
        isSubmissionOk: false,
        formData: {
          businessId: "YohanJS",
          name: "",
          email: "",
          phone: "",
          street: "",
          city: "",
          state: "",
          zip: "",
          service: "",
          meetingType: "",
          date: "",
          time: "",
        },
        services: [
          {
            name: "Modern Pergola",
            description: "A sleek, contemporary pergola to enhance your outdoor living space.",
            image: "/assets/pergola.png",
          },
          {
            name: "Outdoor Kitchens",
            description: "Fully equipped outdoor kitchens for dining and entertaining.",
            image: "/assets/outdoorKitchen.png",
          },
          {
            name: "Motorized Curtains",
            description: "Convenient motorized curtains for outdoor or indoor use.",
            image: "/assets/curtain.png",
          },
          {
            name: "Fences and Gates",
            description: "Durable and stylish fences and gates for added privacy and security.",
            image: "/assets/fencesAndGates.png",
          },
          {
            name: "Carports",
            description: "Protect your vehicles with a custom-designed carport.",
            image: "/assets/carport.png",
          },
          {
            name: "Decks",
            description: "Beautiful and durable decks to elevate your outdoor experience.",
            image: "/assets/deck.png",
          },
        ],
      };
    },
    computed: {
      formattedTime() {
        // Format as HH:MM AM/PM
        if (this.selectedTime === "" || this.selectedTime === null) {
          return "Select time"
        }
        return moment(this.selectedTime, 'HH:mm').format('h:mm A');
      },
      formattedDate() {
        return moment(this.selectedDate).format('MM/DD/YYYY dddd');
      }
    },
    async created() {
      //this.$nextTick() ensures that any DOM updates triggered by changes to this.services are completed before proceeding.
      await this.$nextTick();
      const container = document.getElementById("bookingFormContainer");
      container.scrollIntoView({ behavior: "smooth", block: "start" });

      this.CreateBookingUrl = this.isPrd ? "https://engfuel.com/Bookings/CreateBooking" : "https://localhost:7144/Bookings/CreateBooking";
      this.GetTimes = this.isPrd ? "https://engfuel.com/Bookings/GetTimes" : "https://localhost:7144/Bookings/GetTimes";
    },
    methods: {
      validateFormData(formData) {
        for (const key in formData) {
          const value = formData[key];

          if (typeof value === 'object') {
            // Recursively validate nested objects
            if (this.validateFormData(value)) {
              return true;
            }
          } else if (value === null || value === '') {
            alert(`Please fill in the ${key} field.`);
            return false;
          }
        }
      },
      selectService(service) {
        this.formData.service = service.name;
        this.goToStep(2);
      },
      inPersonMeeting(bool) {
        if (bool === true) {
          this.formData.meetingType = "In Person";
          this.isInPerson = true;
        } else if (bool === false) {
          this.isInPerson = false;
          this.formData.meetingType = "Online/Phone";
        }
        this.goToStep(3);
      },
      goToStep(stepNumber) {
        // Check if the step has changed
        if (this.step !== stepNumber) {
          // Scroll up to the top of the page
          window.scrollTo({ top: 0, left: 0, behavior: 'smooth'});
          // Clear previous step's styles
          var previousStep = document.getElementById("step" + this.step);
          previousStep.classList.remove('fw-bold', 'steel-blue-color');
        }

        // Set the new step
        this.step = stepNumber;

        // Add styles to the current step
        var step = document.getElementById("step" + stepNumber);
        step.classList.add('fw-bold', 'steel-blue-color', 'bg-danger');
      },
      getDates() {
        const dates = [];
        const startOfMonth = moment([this.currentYear, this.currentMonth]); // Start of the current month
        const endOfMonth = moment(startOfMonth).endOf('month'); // End of the current month

        // Add days from the previous month to fill the first week
        const startDay = startOfMonth.day(); // 0 (Sunday) to 6 (Saturday)
        for (let i = 0; i < startDay; i++) {
          dates.push(moment(startOfMonth).subtract(startDay - i, 'days').toDate());
        }

        // Add all dates from the current month
        for (let i = 0; i < endOfMonth.date(); i++) {
          dates.push(moment(startOfMonth).add(i, 'days').toDate());
        }

        // Add days from the next month to complete the grid
        const remainingDays = 7 - (dates.length % 7);
        for (let i = 1; i <= remainingDays && remainingDays < 7; i++) {
          dates.push(moment(endOfMonth).add(i, 'days').toDate());
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
        this.formData.date = moment(date).format('YYYY/MM/DD');
        this.fetchTimes(moment(date).format('YYYY/MM/DD'));
        this.goToStep(4);
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
      generateTimeSlots() {
        const slots = [];
        const startTime = moment().startOf('day').hour(8); // Start at 8:00 AM
        const endTime = moment().startOf('day').hour(18); // End at 6:00 PM
        while (startTime <= endTime) {
          slots.push(startTime.format('h:mm A'));
          startTime.add(15, 'minutes'); // Increment by 15 minutes
        }
        return slots;
      },
      selectTime(time) {
        this.selectedTime = time;
        this.formData.time = time;
        this.goToStep(5);
      },
      async fetchTimes(date) {
        await axios.get(this.GetTimes, { params: { serviceDate: date } })
          .then((response) => {
            this.takenTimes = response.data;
          });
      },
      async submitForm() {
        try {
          this.displaySpinnerMessage = true;
          const response = await axios
            .post(
              this.CreateBookingUrl,
              {
                BusinessId: this.formData.businessId,
                Name: this.formData.name,
                Email: this.formData.email,
                Phone: this.formData.phone,
                Street: this.formData.street,
                City: this.formData.city,
                State: this.formData.state,
                ZipCode: this.formData.zip,
                Service: this.formData.service,
                date: this.formData.date,
                Time: this.formData.time,
              },
            )
            .then(this.isSubmissionOk = true);

          const modalElement = document.getElementById("submissionModal");
          const submissionModal = new Modal(modalElement);

          if (this.isSubmissionOk) {
            submissionModal.show();
          }
        } catch (error) {
          console.log("Failed to submit form. Please try again.");
        }
        // Clear the form
        this.step = 1;
        this.formData = {
          name: "",
          email: "",
          phone: "",
          street: "",
          city: "",
          state: "",
          zip: "",
          service: "",
          date: "",
          time: "",
        };
        this.displaySpinnerMessage = false;
        // Handle form submission (e.g., send to an API)
        //alert(`Booking submitted: \n${JSON.stringify(this.formData, null, 2)}`);
      },
    },
  };
</script>

<!--CSS-->
<style scoped>
  .f-s {
    font-size: 0.8rem;
  }
  .orange-txt {
    color: #F28C28;
  }
  .max-w {
    max-width: 300px;
  }

  .my-bg {
    background-color: #F28C28 !important;
  }
  .small-btn {
      font-size: 0.7rem !important;
  }
  .form-check-input {
    background-color: #000000 !important;
    border: 1px solid #2C3539;
  }

  p, label {
    font-size: 0.9rem;
  }

  .steel-blue-color {
    color: #4682B4;
  }
  .box-shadow {
    box-shadow: 2px 2px 5px rgba(200, 200, 200, 0.3);
  }
  .service-card {
    font-size: 0.8rem;
    background-size: cover; /* Ensure the background image covers the entire card */
    background-position: center; /* Center the image */
    position: relative; /* Required for overlay positioning */
    min-height: 150px; /* Adjust the height as needed */
    box-shadow: 2px 2px 5px rgba(200, 200, 200, 0.3);
  }
  .service-card .overlay {
    background-color: rgba(0, 0, 0, 0.5);
    position: absolute;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
  }
  .card-header {
      font-size: 1.2rem;
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
    background-color: #001524;
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

  .price {
    font-size: 0.7rem;
  }
  /*InPerson/Online CSS*/
  .datepicker-container {
    max-width: 300px;
    margin: 0 auto;
    font-family: Arial, sans-serif;
  }

  .calendar {
    color: #ffffff;
    font-size: 0.8rem;
    padding: 10px;
    box-shadow: 2px 2px 5px rgba(200, 200, 200, 0.3);
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
    font-size: 25px;
    padding: 5px;
    color: #F28C28;
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
    padding: 8px;
    border-radius: 50%;
    transition: background-color 0.3s ease;
  }

  .date:hover {
    background-color: #4682B4;
  }

  .current-date {
    background-color: #ffffff;
    color: #000000;
  }

  .selected-date {
    background-color: #4682B4;
    color: #fff;
  }
  /*TIME PICKER CSS*/
  .timepicker-container {
    padding: 10px;
    box-shadow: 2px 2px 5px rgba(200, 200, 200, 0.3);
    max-width: 300px;
  }

  .time-grid {
    display: grid;
    grid-template-columns: repeat(3, 1fr); /* 3 boxes per row */
    gap: 5px;
  }

  .time-box {
    font-size: 0.7rem;
    text-align: center;
    border: 1px solid #ddd;
    border-radius: 4px;
    color: #ffffff;
    cursor: pointer;
    transition: background-color 0.3s;
  }

    .time-box.taken {
      background-color: #b15454;
      cursor: not-allowed;
      color: #ffffff;
    }

    .time-box.selected {
      background-color: #4682B4;
      color: white;
    }

    .time-box:hover:not(.taken):not(.selected) {
      background-color: #FF2400;
    }
  /*MODAL*/
  .modern-modal {
      background-color: #001524;
      color: #fff;
  }

  .modal-title {
    font-size: 1.5rem;
    color: #ffffff;
  }

  .modal-body {
    font-family: "Roboto", sans-serif;
    line-height: 1.6;
  }

  .btn-close {
    filter: invert(1); /* Makes the close button white for dark backgrounds */
  }

  .shadow-lg {
    box-shadow: 0 8px 16px rgba(0, 0, 0, 0.2);
  }

  .rounded-3 {
    border-radius: 1rem !important;
  }
</style>


