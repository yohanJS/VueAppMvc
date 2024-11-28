<template>
  <nav class="navbar bg-dark-subtle pt-5 pb-5 m-2 rounded-1 shadow-lg navbar-expand-lg">
    <div class="container-fluid">
      <a class="navbar-brand" href="#">BookAI</a>
      <button class="navbar-toggler" type="button" data-bs-toggle="offcanvas" data-bs-target="#offcanvasNavbar" aria-controls="offcanvasNavbar">
        <i class="bi bi-list"></i>
      </button>
      <div class="offcanvas offcanvas-end" tabindex="-1" id="offcanvasNavbar" aria-labelledby="offcanvasNavbarLabel">
        <div class="offcanvas-header">
          <p class="offcanvas-title" id="offcanvasNavbarLabel">BookAI</p>
          <button type="button" class="btn-close" data-bs-dismiss="offcanvas" aria-label="Close"></button>
        </div>
        <div class="offcanvas-body">
          <ul class="navbar-nav justify-content-end flex-grow-1 pe-3">
            <li class="nav-item">
              <a class="nav-link active" aria-current="page" href="#">Home</a>
            </li>
            <li class="nav-item">
              <a class="nav-link" href="#">Link</a>
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
