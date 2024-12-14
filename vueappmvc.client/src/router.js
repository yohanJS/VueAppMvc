import { createRouter, createWebHistory } from 'vue-router';
import Home from './components/Home.vue'
import Login from './components/Login.vue'
import Register from './components/Register.vue'
import FetchData from './components/FetchData.vue'
import BookingForm from './components/BookingForm.vue'

const routes = [
  { path: '/', component: Home, name: 'Home' },
  { path: '/Login', component: Login, name: 'Login' },
  { path: '/Register', component: Register, name: 'Register' },
  { path: '/UpcomingBookings', component: FetchData, name: 'UpcomingBookings' },
  { path: '/BookingForm', component: BookingForm, name: 'Book' },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;
