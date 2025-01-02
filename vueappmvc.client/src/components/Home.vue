<template>
  <div class="home-container d-flex flex-column justify-content-center align-items-center text-center">
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
    position: relative; /* Required for proper pseudo-element placement */
    min-height: 100vh;
    background-color: #001524;
    padding: 2rem;
    display: flex;
    justify-content: center;
    align-items: center;
    text-align: center;
    overflow: hidden; /* Ensure the pseudo-element doesn't overflow */
  }

    /* Add background image with opacity */
    .home-container::before {
      content: "";
      position: absolute;
      top: 0;
      left: 0;
      right: 0;
      bottom: 0;
      background-image: url('/assets/bg.png'); /* Path to the background image */
      background-size: cover; /* Cover the container */
      background-position: center; /* Center the image */
      opacity: 0.4; /* Adjust opacity for better contrast */
      z-index: 1; /* Place behind other content */
    }

  p, .btn-book-now {
    position: relative; /* Place above the pseudo-element */
    z-index: 2;
  }

  p {
    font-size: 1.2rem;
    color: #fff;
    background: rgba(255, 255, 255, 0.1); /* Glass effect */
    backdrop-filter: blur(10px); /* Add blur to the background */
    padding: 1rem;
    border-radius: 10px;
    box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1); /* Optional shadow for depth */
  }

  .btn-book-now {
    background-color: rgba(242, 140, 40, 0.7); /* Semi-transparent background */
    color: #ffffff;
    padding: 0.75rem 4.5rem;
    border-radius: 25px;
    transition: background-color 0.3s ease;
    backdrop-filter: blur(10px); /* Glass effect */
    border: none;
    box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1); /* Subtle shadow for depth */
  }

    .btn-book-now:hover {
      background-color: rgba(0, 0, 0, 0.8);
      color: white;
    }
</style>

