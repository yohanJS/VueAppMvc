<template>
  <div class="home-container d-flex flex-column justify-content-center align-items-center text-center">
    <h2 class="display-4 mb-3">Welcome to Our App</h2>
    <p class="lead mb-4">
      Plan, book, and manage your appointments with ease.
    </p>
    <router-link to="/BookingForm" class="btn btn-book-now">
      Book Now
    </router-link>
  </div>
</template>

<style scoped>
  .home-container {
    min-height: 100vh;
    background: linear-gradient(135deg, #f9f7f7, #dbe2ef);
    padding: 2rem;
    color: #112d4e;
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
    background-color: #f8b195;
    color: white;
    font-weight: bold;
    padding: 0.75rem 1.5rem;
    border-radius: 25px;
    transition: background-color 0.3s ease;
  }

    .btn-book-now:hover {
      background-color: #f67280;
      color: white;
    }
</style>
