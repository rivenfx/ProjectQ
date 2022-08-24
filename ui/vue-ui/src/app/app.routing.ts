import { RouteRecordRaw } from "vue-router";
import dashboard from "./dashboard/dashboard.vue";

const AppRouting: RouteRecordRaw[] = [
  {
    path: '/app',
    name: 'app',
    component: () => import('./app.vue'),
    redirect: '/app/dashboard',
    children: [
      {
        path: 'dashboard',
        name: 'dashboard',
        component: () => import('./dashboard/dashboard.vue')
      }
    ]
  },
  {
    path: '/dashboard',
    name: 'dashboard',
    component: dashboard
  }
]

export { AppRouting };
