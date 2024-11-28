<template>
  <nav class="navbar bg-nav pt-2 pb-2 fixed-top navbar-expand-lg">
    <div class="container-fluid">
      <a class="navbar-brand text-white" href="#">BookAI</a>
      <button class="navbar-toggler" type="button" data-bs-toggle="offcanvas" data-bs-target="#offcanvasNavbar" aria-controls="offcanvasNavbar">
        <i class="bi bi-list text-white"></i>
      </button>
      <div class="offcanvas offcanvas-end bg-nav" tabindex="-1" id="offcanvasNavbar" aria-labelledby="offcanvasNavbarLabel">
        <div class="offcanvas-header">
          <p class="offcanvas-title text-white" id="offcanvasNavbarLabel">BookAI</p>
          <button type="button" class="btn-close" data-bs-dismiss="offcanvas" aria-label="Close"></button>
        </div>
        <div class="offcanvas-body">
          <ul class="navbar-nav justify-content-end flex-grow-1 pe-3">
            <li class="nav-item">
              <a class="nav-link active text-white" aria-current="page" href="#">Home</a>
            </li>
            <li class="nav-item">
              <a class="nav-link text-white" href="#">Link</a>
            </li>
          </ul>
        </div>
      </div>
    </div>
  </nav>
</template>

<script setup>

</script>

<style scoped>
  .bg-nav {
      background-color: #2C3539;
  }
  .navbar-toggler {
    border: none !important;
    box-shadow: none !important;
  }
  .navbar-brand {
      font-size: 0.9rem;
  }
  .offcanvas-title {
    font-size: 0.9rem;
  }
</style>
