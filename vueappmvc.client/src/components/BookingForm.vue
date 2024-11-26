<template>
  <div class="container py-4 mt-5">
    <h6 class="mb-4 text-center">Book Your Service</h6>
    <form @submit.prevent="submitForm">
      <div v-if="step === 1">
        <!-- Step 1: Booking Details -->
        <div class="mb-2">
          <label for="service" class="form-label">Select a Service</label>
          <select id="service"
                  class="form-select"
                  v-model="formData.service"
                  required>
            <option value="" disabled>Select a service</option>
            <option v-for="service in services"
                    :key="service.id"
                    :value="service.name">
              {{ service.name }} - ${{ service.price }}
            </option>
          </select>
          <p v-if="formData.service" class="mt-2 text-muted">
            {{ getSelectedServiceDescription() }}
          </p>
        </div>

        <div class="mb-2">
          <label for="date" class="form-label">Booking Date</label>
          <input type="date"
                 id="date"
                 class="form-control"
                 v-model="formData.date"
                 required />
        </div>

        <div class="mb-2">
          <label for="time" class="form-label">Booking Time</label>
          <input type="time"
                 id="time"
                 class="form-control"
                 v-model="formData.time"
                 required />
        </div>
        <button type="button" class="btn btn-outline-dark w-100" @click="goToStep(2)">
          <i class="bi bi-arrow-right-circle"></i>
        </button>
      </div>

      <div v-if="step === 2">
        <!-- Step 2: Personal Details -->
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
                  class="btn btn-outline-secondary w-25"
                  @click="goToStep(1)">
            <i class="bi bi-arrow-left-circle"></i>
          </button>
          <button type="submit" class="btn btn-outline-primary w-25">
            <i class="bi bi-send-check-fill"></i>
          </button>
        </div>
      </div>
    </form>
  </div>
</template>

<script>
  export default {
    name: "BookingForm",
    data() {
      return {
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
    methods: {
      getSelectedServiceDescription() {
        const selectedService = this.services.find(
          (service) => service.name === this.formData.service
        );
        return selectedService ? selectedService.description : "";
      },
      goToStep(stepNumber) {
        this.step = stepNumber;
      },
      submitForm() {
        // Handle form submission (e.g., send to an API)
        alert(`Booking submitted:\n${JSON.stringify(this.formData, null, 2)}`);

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
    },
  };
</script>

<style scoped>
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
</style>
