<template>
  <div class="container d-flex justify-content-center align-items-center vh-100">
    <div class="card p-4 shadow-sm rounded-4" style="max-width: 360px; width: 100%; background-color: #f8b195;">
      <h2 class="text-center mb-4" style="color: #3f72af; font-size: 1.2rem;">Login</h2>
      <form @submit.prevent="login">
        <div class="mb-3">
          <input type="email"
                 v-model="email"
                 placeholder="Email"
                 class="form-control rounded-pill border-0 py-2"
                 style="font-size: 0.8rem;" />
        </div>
        <div class="mb-3">
          <input type="password"
                 v-model="password"
                 placeholder="Password"
                 class="form-control rounded-pill border-0 py-2"
                 style="font-size: 0.8rem;" />
        </div>
        <button type="submit"
                class="btn w-100 rounded-pill py-2 text-white"
                style="background-color: #3f72af; font-size: 0.8rem;">
          Login
        </button>
      </form>
      <p class="text-center mt-3 mb-0" style="font-size: 0.8rem; color: #3f72af;">{{ message }}</p>
    </div>
  </div>
</template>

<script>
  import axios from "axios";

  export default {
    data() {
      return {
        email: "",
        password: "",
        message: "",
      };
    },
    methods: {
      async login() {
        try {
          const response = await axios.post("/api/account/login", {
            email: this.email,
            password: this.password,
          });
          this.message = response.data.message;
        } catch (error) {
          this.message = error.response.data.message;
        }
      },
    },
  };
</script>

<style scoped>
  body {
    background-color: #f2f2f2;
    font-family: Arial, sans-serif;
    font-size: 0.8rem;
  }
</style>
