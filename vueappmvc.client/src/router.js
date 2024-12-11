import { createRouter, createWebHistory } from 'vue-router';
import FetchData from './components/FetchData.vue'
import BookingForm from './components/BookingForm.vue'

const routes = [
  { path: '/UpcomingBookings', component: FetchData, name: 'UpcomingBookings' },
  { path: '/BookingForm', component: BookingForm, name: 'Book' },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;
