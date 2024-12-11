<script setup>
  import NavBar from './components/NavBar.vue'
  import FetchData from './components/FetchData.vue'
  import BookingForm from './components/BookingForm.vue'
</script>

<template>
  <header>
    <router-link to="/UpcomingBookings" class="nav-link text-dark">Bookings</router-link>
    <router-link to="/BookingForm" class="nav-link text-dark">Book Now</router-link>
  </header>

  <main>
    <!--router-view is where the matched component will be rendered based on the URL.-->
    <router-view></router-view>
  </main>
</template>

<style scoped>
header {
  line-height: 1.5;
}

.logo {
  display: block;
  margin: 0 auto 2rem;
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
