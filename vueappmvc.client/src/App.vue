<script setup>
  import NavBar from './components/NavBar.vue'
  import FetchData from './components/FetchData.vue'
  import BookingForm from './components/BookingForm.vue'
</script>

<template>
  <header class="mobile-nav">
    <ul class="nav nav-tabs justify-content-center p-2 fixed-bottom bg-steel m-1 rounded-2 shadow-lg">
      <li class="nav-item">
        <router-link to="/" class="nav-link">
          <i class="bi bi-calendar-check"></i>
          Home
        </router-link>
      </li>
      <li class="nav-item">
        <router-link to="/UpcomingBookings" class="nav-link">
          <i class="bi bi-calendar-check"></i>
          Bookings
        </router-link>
      </li>
      <li class="nav-item">
        <router-link to="/BookingForm" class="nav-link">
          <i class="bi bi-pencil-square"></i>
          Book Now
        </router-link>
      </li>
    </ul>
  </header>

  <main>
    <!--router-view is where the matched component will be rendered based on the URL.-->
    <router-view></router-view>
  </main>
</template>

<style scoped>
  .bg-steel {
    background-color: #4682B4;
  }
  header {
    line-height: 1.5;
  }

  .logo {
    display: block;
    margin: 0 auto 2rem;
  }
  /* Mobile navigation bar styling */
  .mobile-nav {
    position: sticky;
    bottom: 0;
    width: 100%;
    z-index: 1000;
  }

  /* Style for the nav links */
  .nav-link {
    color: #6c757d; /* Neutral gray for icons and text */
    font-weight: 500;
    font-size: 0.7rem;
    text-align: center;
    padding: 0.5rem;
    background-color: #f1f3f5; /* Light pastel background */
    border-radius: 0.5rem;
    margin: 0 0.25rem;
    transition: all 0.3s ease;
  }

  /* Active and hover states */
  .mobile-nav .nav-link.active,
  .mobile-nav .nav-link:hover {
    background-color: #dbeafe; /* Light pastel blue */
    color: #1d4ed8; /* Dark blue for text and icons */
  }

  /* Icons styling */
  .mobile-nav .nav-link i {
    display: block;
    font-size: 1.2rem;
    margin-bottom: 0.25rem;
  }

  /* Utility for fixed-bottom behavior */
  @media (min-width: 768px) {
    .mobile-nav {
      position: relative;
    }
  }

  @media (min-width: 1024px) {
    header {
      display: flex;
      place-items: center;
      padding-right: calc(var(--section-gap) / 2);
    }

    .logo {
      margin: 0 2rem 0 0;
    }

    header .wrapper {
      display: flex;
      place-items: flex-start;
      flex-wrap: wrap;
    }
  }
</style>
