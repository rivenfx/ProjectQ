import { createRouter, createWebHashHistory, createWebHistory, RouteRecordRaw } from 'vue-router';
import { AppRouting } from './app';


const AppRootRouting = createRouter({
  history: createWebHashHistory(import.meta.env.VITE_PUBLIC_PATH),
  routes: [
    ...AppRouting
  ],
  strict: true,
  scrollBehavior: () => ({ left: 0, top: 0 }),
});

// app router
export { AppRootRouting };
