<!--Template-->
<template>
  <div class="container py-4 mt-5">
    <div class="text-center mb-4 mt-2">
      <div class="px-5" role="group" aria-label="First group">
        <button @click="goToStep(1)" id="step1" type="button" class="btn btn-primary m-1 rounded-5 my-bg border-0 fw-bold steel-blue-color">1</button>
        <button @click="goToStep(2)" id="step2" type="button" class="btn btn-primary m-1 rounded-5 my-bg border-0">2</button>
        <button @click="goToStep(3)" id="step3" type="button" class="btn btn-primary m-1 rounded-5 my-bg border-0">3</button>
        <button @click="goToStep(4)" id="step4" type="button" class="btn btn-primary m-1 rounded-5 my-bg border-0">4</button>
        <button @click="goToStep(5)" id="step5" type="button" class="btn btn-primary m-1 rounded-5 my-bg border-0">5</button>
      </div>
    </div>

    <div v-if="step === 5 && formData.service !== ''" class="bg-success-subtle p-2 mb-3 rounded-1 shadow-sm mt-2">
      <p class="mb-2 text-center steel-blue-color">Your appointment details:</p>
      <ul class="list-group list-group-flush rounded-2">
        <li class="list-group-item f-s"><span class="fw-bold">Meeting Type:</span> {{ formData.meetingType }}</li>
        <li class="list-group-item f-s"><span class="fw-bold">Service:</span> {{ formData.service }}</li>
        <li class="list-group-item f-s"><span class="fw-bold">Date:</span> {{ formattedDate }}</li>
        <li class="list-group-item f-s"><span class="fw-bold">Time:</span> {{ formattedTime }}</li>
      </ul>
    </div>

    <form @submit.prevent="submitForm">
      <!-- Step 1: Booking Service -->
      <div v-if="step === 1">
        <p class="text-center mb-1 steel-blue-color">Quote service</p>
        <div class="d-flex flex-column gap-3">
          <div v-for="service in services"
               :key="service.id"
               class="service-card p-2 rounded-2 shadow-sm btn text-white text-start"
               @click="selectService(service)">
            <div class="card-header">
              <p class="m-0 lead fw-bold steel-blue-color">{{ service.name }}</p>
            </div>
            <p class="mb-2">{{ service.description }}</p>
            <span class="price">Get quote</span>
            <div class="text-end">
              <i class="bi bi-arrow-right-circle"></i>
            </div>
          </div>
        </div>
      </div>

      <!-- Step 2 In Peson/Online/Phone-->
      <div v-if="step === 2" class="text-center">
        <p class="text-center mb-1 steel-blue-color">Type of meeting</p>
        <button @click="inPersonMeeting(true)" class="btn btn-outline-primary my-bg border-0 w-50 mb-2 text-white">
          <i class="bi bi-person m-2"></i>In Person
        </button>
        <br />
        <button @click="inPersonMeeting(false)" class="btn btn-outline-primary my-bg border-0 w-50 text-white">
          <i class="bi bi-telephone-forward m-2"></i>Online/Phone
        </button>
      </div>

      <!-- Step 3: Date -->
      <div v-if="step === 3">
        <p class="text-center mb-1 steel-blue-color">Select date</p>
        <div class="datepicker-container m-2 mx-auto">
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

        <!--<div class="d-flex justify-content-between my-bg rounded-2 shadow-sm mx-auto" style="max-width: 300px;">
          <button type="button"
                  class="btn w-25"
                  @click="goToStep(2)">
            <i class="bi bi-arrow-left-circle text-white"></i>
          </button>
          <button type="button"
                  class="btn w-25"
                  @click="goToStep(4)">
            <i class="bi bi-arrow-right-circle text-white"></i>
          </button>
        </div>-->
      </div>

      <!-- Step 4: Time Picker -->
      <div v-if="step === 4">
        <p class="text-center mb-1 steel-blue-color">Select time</p>
        <div class="timepicker-container m-2 mx-auto">
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
        <p class="text-center mb-1 steel-blue-color">Enter personal details</p>
        <!--In Person-->
        <div v-if="isInPerson === true" class="my-bg p-2 rounded-2 shadow-sm">

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
                   v-model="formData.address.street"
                   placeholder=""
                   required />
          </div>

          <div class="row mb-2">
            <div class="col">
              <label for="city" class="form-label text-white">City</label>
              <input type="text"
                     id="city"
                     class="form-control"
                     v-model="formData.address.city"
                     placeholder=""
                     required />
            </div>
            <div class="col">
              <label for="state" class="form-label text-white">State</label>
              <input type="text"
                     id="state"
                     class="form-control"
                     v-model="formData.address.state"
                     placeholder=""
                     required />
            </div>
            <div class="col">
              <label for="zip" class="form-label text-white">ZIP Code</label>
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
                    @click="goToStep(4)">
              <i class="bi bi-arrow-left-circle text-white"></i>
            </button>
            <button type="submit" class="btn w-25">
              <i class="bi bi-send-check-fill text-white"></i>
            </button>
          </div>
        </div>
        <!--Online/Phone-->
        <div v-if="isInPerson === false" class="my-bg p-2 rounded-2 shadow-sm">

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

          <div class="d-flex justify-content-between">
            <button type="button"
                    class="btn w-25"
                    @click="goToStep(2)">
              <i class="bi bi-arrow-left-circle text-white"></i>
            </button>
            <button type="submit" class="btn w-25">
              <i class="bi bi-send-check-fill text-white"></i>
            </button>
          </div>

        </div>
      </div>
    </form>
    <!--SUCCESS MODAL-->
    <div>
      <div class="modal fade" id="submissionModal" tabindex="-1" aria-labelledby="submissionModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
          <div class="modal-content modern-modal rounded-3 border-0 shadow-lg">
            <div class="modal-header border-0">
              <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body text-center p-4">
              <h5 class="modal-title fw-bold mb-3">Thank You!</h5>
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
        currentYear: today.getFullYear(),
        currentMonth: today.getMonth(),
        selectedDate: today,
        weekdays: ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'],
        monthNames: [
          'January', 'February', 'March', 'April', 'May', 'June',
          'July', 'August', 'September', 'October', 'November', 'December'
        ],
        timeSlots: this.generateTimeSlots(),
        selectedTime: null,
        takenTimes: ["9:00 AM", "9:15 AM", "12:00 PM"],
        step: 1,
        isInPerson: false,
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
          meetingType: "",
          date: "",
          time: "",
        },
        services: [
          {
            name: "Modern Pergola",
            description: "A sleek, contemporary pergola to enhance your outdoor living space.",
          },
          {
            name: "Outdoor Kitchens",
            description: "Fully equipped outdoor kitchens for dining and entertaining.",
          },
          {
            name: "Motorized Curtains",
            description: "Convenient motorized curtains for outdoor or indoor use.",
          },
          {
            name: "Fences and Gates",
            description: "Durable and stylish fences and gates for added privacy and security.",
          },
          {
            name: "Carports",
            description: "Protect your vehicles with a custom-designed carport.",
          },
          {
            name: "Decks",
            description: "Beautiful and durable decks to elevate your outdoor experience.",
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
        this.formData.service = service.name; // Set the service name in formData
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
        console.log(this.formData.meetingType);
      },
      goToStep(stepNumber) {
        if (this.step !== stepNumber) {
          //Clears CSS
          var previousStep = document.getElementById("step" + this.step);
          previousStep.classList.remove('fw-bold', 'steel-blue-color');
        }
        this.step = stepNumber;
        var step = document.getElementById("step" + stepNumber);
        step.classList.add('fw-bold', 'steel-blue-color');
      },
      submitForm() {
        const modalElement = document.getElementById("submissionModal");
        const submissionModal = new Modal(modalElement);
        console.log(this.formData);
        console.log("Form submitted....");
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
        //if (true) {
        //  submissionModal.show();
        //  console.log(this.formData);
        //}
        // Handle form submission (e.g., send to an API)
        //alert(`Booking submitted: \n${JSON.stringify(this.formData, null, 2)}`);
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
        this.formData.date = moment(date).format('YYYY/MM/DD');
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
    },
  };
</script>
<!--CSS-->
<style scoped>
  .f-s {
    font-size: 0.8rem;
  }

  .max-w {
    max-width: 300px;
  }

  .my-bg {
    background-color: #2C3539 !important;
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

  .service-card {
    border-bottom: 2px solid #2C3539;
    background-color: #2C3539;
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
    padding: 10px;
    background-color: #2C3539;
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
    color: #4682B4;
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
    max-width: 300px;
  }

  .time-grid {
    display: grid;
    grid-template-columns: repeat(3, 1fr); /* 3 boxes per row */
    gap: 7px;
  }

  .time-box {
    font-size: 0.8rem;
    text-align: center;
    border: 1px solid #ddd;
    border-radius: 4px;
    background-color: #2C3539;
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
    background: linear-gradient(135deg, #228B22, #1B5E20);
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


