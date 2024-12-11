<template>
  <div class="container mt-4">
    <!-- Bookings Section -->
    <div class="mt-5">
      <div class="text-center mb-4">
        <h1 class="lead">Upcoming bookings</h1>
      </div>

      <!-- Button to Fetch Bookings -->
      <div class="text-center">
        <button class="btn btn-success btn-sm" @click="fetchBookings">
          Get Latest
        </button>
      </div>

      <!-- Loading State -->
      <div v-if="loading" class="text-center text-success-emphasis">
        Loading bookings... Please wait.
      </div>

      <!-- Display Bookings -->
      <div v-if="users && users.length > 0" class="row g-4 justify-content-center">
        <div v-for="user in users" :key="user.id" class="col-12 col-md-6 col-lg-4 d-flex align-items-stretch">
          <div class="pastel-card card h-100 w-100 shadow-sm">
            <div class="card-body d-flex flex-column">
              <h5 class="card-title">{{ user.name }}</h5>
              <p class="card-text"><strong>Email:</strong> {{ user.email }}</p>
              <p class="card-text"><strong>Phone:</strong> {{ user.phone }}</p>
              <p class="card-text">
                <strong>Address:</strong>
                <div v-if="user.street === null || user.street === ''">
                  N/A
                </div>
                <div v-else>
                  {{ user.street }}, {{ user.city }}, {{ user.state }} - {{ user.zip }}
                </div>
              </p>
              <h6 class="mt-3">Services:</h6>
              <ul class="list-group list-group-flush">
                <li v-for="service in user.services" :key="service.id" class="pastel-list-item list-group-item">
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
          const response = await fetch("http://engfuel.com/Bookings");
          //const response = await fetch("https://localhost:7144/Bookings");
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
      padding: 1.5rem;
      background-color: #ffffff;
      border-radius: 16px;
      flex: 1; /* Make the body fill available space */
      display: flex;
      flex-direction: column;
    }

    /* Card title styling */
    .pastel-card .card-title {
      color: #6a5acd; /* Pastel blue-violet */
      font-size: 1.2rem; /* Slightly smaller font for mobile look */
      font-weight: 700;
      margin-bottom: 1rem;
    }

    /* Card text styling */
    .pastel-card .card-text {
      color: #555;
      font-size: 0.9rem; /* Smaller font for a modern mobile look */
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
    color: #9b59b6; /* Soft pastel purple */
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

  /* Responsive spacing adjustments */
  @media (max-width: 768px) {
    .pastel-card .card-body {
      padding: 1.2rem; /* Smaller padding on smaller screens */
    }

    .pastel-card .card-title {
      font-size: 1rem; /* Smaller font size for mobile screens */
    }

    .pastel-card .card-text,
    .pastel-list-item {
      font-size: 0.85rem; /* Smaller text for mobile */
    }
  }

</style>
