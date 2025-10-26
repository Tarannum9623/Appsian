// services/api.ts
import axios from 'axios';

const API_BASE_URL = process.env.REACT_APP_API_URL || 'https://localhost:7008/api';

const api = axios.create({
  baseURL: API_BASE_URL,
});

// Add token to requests
api.interceptors.request.use((config) => {
  const token = localStorage.getItem('token');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

export const authAPI = {
  register: (userData: { username: string; email: string; password: string }) =>
    api.post('/auth/register', userData),
  login: (credentials: { username: string; password: string }) =>
    api.post('/auth/login', credentials),
};

export const projectsAPI = {
  getAll: () => api.get('/projects'),
  getById: (id: number) => api.get(`/projects/${id}`),
  create: (projectData: { title: string; description?: string }) =>
    api.post('/projects', projectData),
  delete: (id: number) => api.delete(`/projects/${id}`),
};

export const tasksAPI = {
  create: (projectId: number, taskData: any) =>
    api.post(`/projects/${projectId}/tasks`, taskData),
  update: (id: number, taskData: any) => api.put(`/tasks/${id}`, taskData),
  delete: (id: number) => api.delete(`/tasks/${id}`),
  schedule: (projectId: number, scheduleData: ScheduleRequest) =>
    api.post(`/projects/${projectId}/tasks/schedule`, scheduleData),
};

export default api;