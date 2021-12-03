import { RouteRecordRaw } from "vue-router";
import { LayoutDefault } from "/@/layout";

const AppRouting: RouteRecordRaw[] = [
  {
    path: '/app',
    component: LayoutDefault,
    redirect: '/app/dashboard',
    children: [
      {
        path: 'dashboard',
        name: 'dashboard',
        component: import('./dashboard/dashboard.vue')
      }
    ]
  }
]

export { AppRouting };
