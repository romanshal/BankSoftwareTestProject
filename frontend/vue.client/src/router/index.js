import { createRouter, createWebHistory } from 'vue-router';
import LoanCreate from '../views/LoanCreate.vue'
import LoanList from '../views/LoanList.vue'

const routes = [
  { path: '/', name: 'loans', component: LoanList },
  { path: '/create', name: 'loan-create', component: LoanCreate }
];

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL || '/'),
  routes
});

export default router;