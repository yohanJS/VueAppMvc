<template>
  <div class="home-container d-flex flex-column justify-content-center align-items-center text-center">
    <!--<h2 class="display-4 mb-3">Welcome to Our App</h2>-->
    <p class="mb-4 text-white fs-2">
      Plan, book, and manage your appointments with ease.
    </p>
    <router-link to="/BookingForm" class="btn btn-sm btn-book-now">
      Book Now
      <i class="bi bi-chevron-double-right"></i>
    </router-link>
  </div>
</template>

<style scoped>
  .home-container {
    min-height: 100vh;
    background-color: #001524;
    padding: 2rem;
  }

  h2 {
    font-weight: bold;
    color: #3f72af;
  }

  p {
    font-size: 1.2rem;
    color: #112d4e;
  }

  .btn-book-now {
    background-color: #F28C28;
    color: #ffffff;
    padding: 0.75rem 4.5rem;
    border-radius: 25px;
    transition: background-color 0.3s ease;
  }

    .btn-book-now:hover {
      background-color: #000;
      color: white;
    }
</style>
