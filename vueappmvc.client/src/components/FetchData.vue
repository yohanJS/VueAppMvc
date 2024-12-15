<template>
  <div class="container mb-5">
    <!-- Bookings Section -->
    <div class="mt-3">
      <!-- Button to Fetch Bookings -->
      <div class="text-end">
        <button @click="fetchBookings"
                class="btn rounded-pill py-2 fw-bold"
                style="background-color: #f8b195; font-size: 0.8rem; ">
          <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" fill="#3f72af" class="bi bi-arrow-clockwise" viewBox="0 0 16 16">
            <path fill-rule="evenodd" d="M8 3a5 5 0 1 0 4.546 2.914.5.5 0 0 1 .908-.417A6 6 0 1 1 8 2z" />
            <path d="M8 4.466V.534a.25.25 0 0 1 .41-.192l2.36 1.966c.12.1.12.284 0 .384L8.41 4.658A.25.25 0 0 1 8 4.466" />
          </svg>
        </button>
      </div>

      <!-- Loading State -->
      <div v-if="loading" class="text-center text-success-emphasis">
        Loading bookings... Please wait.
        <div class="d-flex justify-content-center mt-3">
          <div class="spinner-border" role="status">
            <span class="visually-hidden">Loading...</span>
          </div>
        </div>
      </div>

      <!-- Display Bookings -->
      <div v-if="users && users.length > 0" class="row g-4 justify-content-center">
        <div v-for="user in users" :key="user.id" class="col-12 col-md-6 col-lg-4 d-flex align-items-stretch">
          <div class="pastel-card card h-100 w-100 shadow-sm">
            <div class="card-body d-flex flex-column rounded-2">
              <h4 class="card-title mb-2">{{ user.name }}</h4>
              <p class="card-text mb-1"><strong>Email:</strong> <br /> {{ user.email }}</p>
              <p class="card-text mb-1"><strong>Phone:</strong> <br /> {{ user.phone }}</p>
              <p class="card-text">
                <strong>Address:</strong>
                <div v-if="!user.street">
                  N/A
                </div>
                <div v-else>
                  {{ user.street }}, {{ user.city }}, {{ user.state }} - {{ user.zip }}
                </div>
              </p>

              <!-- Tabs for Services -->
              <ul class="nav nav-tabs mt-3" :id="'serviceTabs' + user.id" role="tablist">
                <li class="nav-item" v-for="(service, index) in user.services" :key="service.id" role="presentation">
                  <button class="nav-link p-2 steel-blue-color" style="font-size:0.7rem;" :class="{ active: index === 0 }" :id="'tab' + user.id + '-' + service.id"
                          data-bs-toggle="tab" :data-bs-target="'#content' + user.id + '-' + service.id" type="button" role="tab"
                          :aria-controls="'content' + user.id + '-' + service.id" :aria-selected="index === 0">
                    {{ service.date }}
                  </button>
                </li>
              </ul>

              <div class="tab-content mt-2" style="font-size: 0.7rem;" :id="'serviceTabContent' + user.id">
                <div v-for="(service, index) in user.services" :key="service.id"
                     class="tab-pane fade" :class="{ 'show active': index === 0 }"
                     :id="'content' + user.id + '-' + service.id" role="tabpanel"
                     :aria-labelledby="'tab' + user.id + '-' + service.id">
                  <p class="mb-1"><strong>Service:</strong> {{ service.service }}</p>
                  <p class="mb-1"><strong>Date:</strong> {{ service.date }}</p>
                  <p class="mb-1"><strong>Time:</strong> {{ service.time }}</p>

                  <i @click="deleteService(service.id)" class="bi bi-trash3 text-danger mr">Delete</i>
                  <i @click="test" class="bi bi-pencil text-primary">Edit</i>
                </div>
              </div>

            </div>
          </div>
        </div>
      </div>

      <!-- No Bookings Message -->
      <div v-else-if="!loading" class="text-center text-danger mt-5">
        <p>No bookings available at the moment. Be the first to book!</p>
      </div>
    </div>
  </div>
</template>

<script>
  import axios from 'axios';

  export default {
    data() {
      return {
        loading: false,
        users: null,
        serviceId: null,
      };
    },
    async created() {
      await this.fetchBookings();
    },
    methods: {
      //async fetchBookings() {
      //  this.users = null;
      //  this.loading = true;
      //  try {
      //    const response = await fetch("http://engfuel.com/Bookings");
      //    //const response = await axios.get("https://localhost:7144/Bookings");
      //    this.users = response.data.users;
      //  } catch (error) {
      //    console.error("Error fetching bookings:", error);
      //  } finally {
      //    this.loading = false;
      //  }
      //},
      async fetchBookings() {
        this.users = null;
        this.loading = true;
        try {
          const response = await fetch("http://engfuel.com/Bookings");
          const data = await response.json(); // Parse the response as JSON
          this.users = data.users;            // Access users from the parsed data
        } catch (error) {
          console.error("Error fetching bookings:", error);
        } finally {
          this.loading = false;
        }
      },
      async deleteService(id) {
        try {
          //const response = await fetch("http://engfuel.com/DeleteBookedService");
          const response = await axios.post(
            "http://engfuel.com/DeleteBookedService",
            {
              serviceId: id,
            }
          );
        } catch (error) {
          console.error("Error deleting bookings:", error);
        } finally {
          this.loading = false;
          this.fetchBookings();
        }
      },
    },
  };
</script>

<style scoped>
  /* General body styling for consistency */
  body {
    font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
    background-color: #f8f9fa;
    color: #333;
    margin: 0;
    padding: 0;
  }

  /* Container for overall layout */
  .row {
    margin: 0 auto;
    max-width: 1200px;
  }

  /* Modern pastel card styling */
  .pastel-card {
    background: linear-gradient(145deg, #fdf7ff, #f5f0fa); /* Soft gradient */
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
      background-color: #ffffff;
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

  /* Strong tag for emphasis */
  .pastel-card strong {
    color: #3f72af;
  }

  /* Centered grid layout */
  .justify-content-center {
    display: flex;
    flex-wrap: wrap;
    justify-content: center;
  }

  /* Ensure all columns stretch equally */
  .row > div {
    display: flex;
  }

  .mr {
    margin-right: 10px;
  }
</style>
