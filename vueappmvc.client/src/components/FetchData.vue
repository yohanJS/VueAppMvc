<template>
  <div class="text-center mt-3">
    <button class="btn btn-outline-dark pt-0 pb-0 mt-5" @click="fetchBookings">
      <span style="font-size: 0.8rem;">Get Latest</span>
    </button>
  </div>

  <!-- Bookings Section -->
  <div class="booking-carousel mt-5">
    <div class="card p-2 border-0 bg-light">
      <h1>Bookings</h1>

      <div v-if="loading" class="loading">Loading bookings... Please wait.</div>

      <div v-if="bookings && bookings.length > 0" class="carousel-container">
        <div class="carousel" :style="{ transform: `translateX(-${currentIndex * 100}%)` }">
          <div v-for="booking in bookings" :key="booking.phone" class="booking-card mx-auto mt-2 mb-2 p-2">
            <div class="booking-header">
              <h3>{{ booking.name }}</h3>
              <div class="booking-details">
                <p><strong>Service:</strong> {{ booking.service }}</p>
                <p><strong>Date:</strong> {{ booking.date }}</p>
                <p><strong>Time:</strong> {{ booking.time }}</p>
                <p><strong>Address:</strong> {{ booking.address.street }}, {{ booking.address.city }}, {{ booking.address.state }} - {{ booking.address.zip }}</p>
              </div>
            </div>
          </div>
        </div>

        <!-- Carousel Navigation -->
        <div class="carousel-navigation">
          <button @click="previousBooking" :disabled="currentIndex === 0">❮</button>
          <button @click="nextBooking" :disabled="currentIndex === bookings.length - 1">❯</button>
        </div>
      </div>

      <div v-else-if="!loading" class="no-bookings">
        <p>No bookings available at the moment. Be the first to book!</p>
      </div>
    </div>
  </div>
</template>

<script lang="js">
  export default {
    data() {
      return {
        total: 2,
        loading: false,
        bookings: null,
        currentIndex: 0
      };
    },
    async created() {
      // fetch the bookings when the view is created
      await this.fetchBookings();
    },
    methods: {
      async fetchBookings() {
        this.bookings = null;
        this.loading = true;

        try {
          //var response = await fetch('http://engfuel.com/GetBookings'); // Modify URL accordingly
          var response = await fetch('https://localhost:7144/GetBookings');
          if (response.ok) {
            this.bookings = await response.json();
          }
        } catch (error) {
          console.error('Error fetching bookings:', error);
        } finally {
          this.loading = false;
        }
      },
      addTwo() {
        this.total += 2;
      },
      clearValue() {
        this.total = 0;
      },
      nextBooking() {
        if (this.currentIndex < this.bookings.length - 1) {
          this.currentIndex++;
        }
      },
      previousBooking() {
        if (this.currentIndex > 0) {
          this.currentIndex--;
        }
      }
    },
  };
</script>

<style scoped>
  /* General component styles */
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
    flex-shrink: 0; /* Prevent shrinking */
    min-width: 280px;
    margin: 0 15px;
    padding: 20px;
    background: #fff;
    border-radius: 8px;
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
    text-align: left;
    transition: all 0.3s ease-in-out;
  }

    .booking-card:hover {
      transform: scale(1.05);
    }

  .booking-header h3 {
    font-size: 20px;
    color: #333;
  }

  .booking-details p {
    font-size: 14px;
    color: #555;
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

  /* Responsive Styles */
  @media (max-width: 992px) {
    .booking-card {
      min-width: 250px;
      margin: 0 10px;
    }

    .carousel-navigation button {
      font-size: 16px;
      padding: 8px 16px;
    }

    .booking-header h3 {
      font-size: 18px;
    }

    .booking-details p {
      font-size: 12px;
    }
  }

  @media (max-width: 768px) {
    .carousel-container {
      flex-direction: column;
      align-items: center;
    }

    .carousel {
      display: block;
    }

    .booking-card {
      min-width: 100%;
      margin-bottom: 20px;
    }

    .carousel-navigation button {
      font-size: 16px;
      padding: 8px 16px;
      margin-bottom: 10px;
    }

    .booking-header h3 {
      font-size: 18px;
    }

    .booking-details p {
      font-size: 14px;
    }

    .no-bookings p {
      font-size: 14px;
    }
  }

  @media (max-width: 480px) {
    .booking-card {
      padding: 15px;
      margin: 0 5px;
    }

    .carousel-navigation button {
      padding: 6px 12px;
    }

    .booking-header h3 {
      font-size: 16px;
    }

    .booking-details p {
      font-size: 12px;
    }

    .no-bookings p {
      font-size: 12px;
    }
  }
</style>
