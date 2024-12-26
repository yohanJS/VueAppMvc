<template>
  <header class="mobile-nav">
    <ul class="nav nav-tabs justify-content-around p-0 fixed-bottom orange-bg rounded-1">
      <li class="nav-item">
        <router-link to="/" class="nav-link p-0 text-white">
          <i class="bi bi-house m-0"></i>
          Home
        </router-link>
      </li>
      <li class="nav-item">
        <router-link to="/UpcomingBookings" class="nav-link p-0 text-white">
          <i class="bi bi-calendar-check m-0"></i>
          Bookings
        </router-link>
      </li>
      <li class="nav-item">
        <router-link to="/BookingForm" class="nav-link p-0 text-white">
          <i class="bi bi-pencil-square m-0"></i>
          Book
        </router-link>
      </li>
      <li class="nav-item" v-if="!isLoggedIn">
        <router-link to="/Login" class="nav-link p-0 text-white">
          <i class="bi bi-person m-0"></i>
          Login
        </router-link>
      </li>
      <li class="nav-item" v-if="!isLoggedIn">
        <router-link to="/Register" class="nav-link p-0 text-white">
          <i class="bi bi-person-add m-0"></i>
          Register
        </router-link>
      </li>
      <li class="nav-item" v-if="isLoggedIn" @click="logout">
        <router-link to="/Login" class="nav-link p-0 text-white">
          <i class="bi bi-escape m-0"></i>
          Log out
        </router-link>
      </li>
    </ul>
  </header>

  <main>
    <!-- The matched component will be rendered here based on the URL -->
    <router-view></router-view>
  </main>
</template>

<script>
  export default {
    data() {
      return {
        isLoggedIn: localStorage.getItem("isLoggedIn") === "true",
      };
    },
    methods: {
      async logout() {
        window.location.href = "http://engfuel.com";
        // Set a flag in localStorage to indicate successful login
        localStorage.setItem("isLoggedIn", "false");
        this.isLoggedIn = false;
      },
    },
  };
</script>

<style scoped>
  /* Steel blue background for the nav bar */
  .orange-bg {
    background-color: #F28C28;
  }

  header {
    line-height: 1.5;
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
    color: #3f72af; /* Neutral gray for icons and text */
    font-weight: 500;
    font-size: 0.7rem;
    text-align: center;
    border: none !important;
    height: 50px; /* Fixed height for buttons */
    display: flex; /* Flexbox for centering content */
    flex-direction: column; /* Stack icon and text vertically */
    justify-content: center; /* Center content vertically */
    align-items: center; /* Center content horizontally */
    margin: 0 0.25rem;
    transition: all 0.3s ease;
  }

    .nav-link:hover {
      color: #3f72af !important;
      background-color: transparent;
    }
  .router-link-active {
    color: #3f72af !important;
  }
  /* Icons styling */
  .mobile-nav .nav-link i {
    font-size: 1.2rem;
    margin-bottom: 0.25rem;
  }

  /* Ensure nav items are evenly distributed */
  .nav-tabs {
    display: flex;
    justify-content: space-around;
  }

  /* Utility for fixed-bottom behavior on larger screens */
  @media (min-width: 768px) {
    .mobile-nav {
      position: relative;
    }
  }
</style>
