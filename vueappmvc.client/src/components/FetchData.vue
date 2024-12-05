<template>
  <div class="container mt-4">
    <!-- Button to Fetch Bookings -->
    <div class="text-center">
      <button class="btn btn-primary" @click="fetchBookings">
        Get Latest
      </button>
    </div>

    <!-- Bookings Section -->
    <div class="mt-5">
      <div class="text-center mb-4">
        <h1 class="text-white-50">Upcoming Bookings</h1>
      </div>

      <!-- Loading State -->
      <div v-if="loading" class="text-center text-success-emphasis">
        Loading bookings... Please wait.
      </div>

      <!-- Display Bookings -->
      <div v-if="users && users.length > 0" class="row g-3">
        <div v-for="user in users" :key="user.id" class="col-12 col-md-6 col-lg-4">
          <div class="card h-100 shadow-sm">
            <div class="card-body">
              <h5 class="card-title">{{ user.name }}</h5>
              <p class="card-text"><strong>Email:</strong> {{ user.email }}</p>
              <p class="card-text"><strong>Phone:</strong> {{ user.phone }}</p>
              <p class="card-text">
                <strong>Address:</strong>
                {{ user.street }}, {{ user.city }}, {{ user.state }} - {{ user.zip }}
              </p>
              <h6 class="mt-3">Services:</h6>
              <ul class="list-group list-group-flush">
                <li v-for="service in user.services" :key="service.id" class="list-group-item">
                  <p><strong>Service:</strong> {{ service.service }}</p>
                  <p><strong>Date:</strong> {{ service.date }}</p>
                  <p><strong>Time:</strong> {{ service.time }}</p>
                </li>
              </ul>
            </div>
          </div>
        </div>
      </div>

      <!-- No Bookings Message -->
      <div v-else-if="!loading" class="text-center text-danger">
        <p>No bookings available at the moment. Be the first to book!</p>
      </div>
    </div>
  </div>
</template>

<script>
  export default {
    data() {
      return {
        loading: false,
        users: null,
      };
    },
    async created() {
      await this.fetchBookings();
    },
    methods: {
      async fetchBookings() {
        this.users = null;
        this.loading = true;
        try {
          const response = await fetch('http://engfuel.com/GetBookings');
          //const response = await fetch("https://localhost:7144/GetBookings");
          if (response.ok) {
            const data = await response.json();
            this.users = data.users;
          }
        } catch (error) {
          console.error("Error fetching bookings:", error);
        } finally {
          this.loading = false;
        }
      },
    },
  };
</script>

<style scoped>
  .booking-carousel {
    display: flex;
    justify-content: center;
    align-items: center;
    padding: 20px;
  }

  .carousel-container {
    display: flex;
    overflow: hidden;
    position: relative;
    width: 100%;
  }

  .carousel {
    display: flex;
    transition: transform 0.5s ease;
  }

  .booking-card {
    flex-shrink: 0;
    min-width: 280px;
    max-width: 400px;
    margin: 10px auto;
    padding: 20px;
    background: #fff;
    border-radius: 12px;
    box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
    text-align: left;
    transition: all 0.3s ease-in-out;
  }

    .booking-card:hover {
      transform: scale(1.05);
    }

  .booking-header h3 {
    font-size: 1.5rem;
    color: #333;
    margin-bottom: 10px;
  }

  .booking-details ul {
    list-style: none;
    padding: 0;
  }

  .booking-details li {
    margin-bottom: 10px;
    border-bottom: 1px solid #ddd;
    padding-bottom: 10px;
  }

  .carousel-navigation {
    display: flex;
    justify-content: center;
    margin-top: 15px;
  }

    .carousel-navigation button {
      background-color: transparent;
      border: 1px solid #007bff;
      color: #007bff;
      font-size: 18px;
      padding: 5px 10px;
      margin: 0 5px;
      border-radius: 5px;
      cursor: pointer;
    }

      .carousel-navigation button:disabled {
        color: #ccc;
        border-color: #ccc;
      }

  .loading {
    font-size: 16px;
    color: #999;
    text-align: center;
  }

  .no-bookings {
    font-size: 16px;
    color: #888;
    margin-top: 20px;
  }

  /* Responsive Design */
  @media (max-width: 768px) {
    .booking-card {
      min-width: 90%;
    }

    .booking-header h3 {
      font-size: 1.25rem;
    }

    .booking-details ul {
      font-size: 0.9rem;
    }

    .carousel-navigation button {
      font-size: 16px;
      padding: 5px 8px;
    }
  }
</style>
