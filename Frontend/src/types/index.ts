// types/index.ts
export interface User {
    id: number;
    username: string;
    email: string;
    token: string;
  }
  
  export interface Project {
    id: number;
    title: string;
    description?: string;
    creationDate: string;
    taskCount: number;
  }
  
  export interface Task {
    id: number;
    title: string;
    description?: string;
    dueDate?: string;
    isCompleted: boolean;
    projectId: number;
    estimatedHours?: number;
    dependencies: string[];
  }
  
  export interface ScheduledTask {
    title: string;
    estimatedHours: number;
    dueDate: string;
    dependencies: string[];
  }
  
  export interface ScheduleRequest {
    tasks: ScheduledTask[];
  }
  
  export interface ScheduleResponse {
    recommendedOrder: string[];
    startDates: { [key: string]: string };
  }