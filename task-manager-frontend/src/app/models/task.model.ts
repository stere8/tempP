// src/app/models/task.model.ts
export interface Task {
    id: number;
    shortDescription: string;
    // Difficulty on a scale from 1 (easiest) to 5 (hardest)
    difficulty: number;
    // Task type can be one of these three types
    taskType: 'Implementation' | 'Deployment' | 'Maintenance';
    // Common fields (for Deployment and Maintenance tasks)
    deadline?: Date;
    // For Deployment tasks (max 400 chars)
    deploymentScope?: string;
    // For Maintenance tasks (max 400 chars each)
    listOfServices?: string;
    listOfServers?: string;
    // For Implementation tasks (up to 1000 chars)
    implementationContent?: string;
    // Task status – you can refine the enum as needed
    status: 'ToDo' | 'Done';
    // Optional field to indicate assignment
    assignedUserId?: number;
  }
  