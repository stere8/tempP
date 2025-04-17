import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Task } from '../models/task.model';

@Injectable({
  providedIn: 'root'
})
export class TaskService {
  private baseUrl = '/api/tasks';

  constructor(private http: HttpClient) { }

  /**
   * Get tasks already assigned to the given user, sorted + limited by backend
   */
  getAssignedTasks(userId: number): Observable<Task[]> {
    return this.http.get<Task[]>(`${this.baseUrl}/assigned/${userId}`);
  }

  /**
   * Get tasks available for assignment to the given user, sorted + limited by backend
   */
  getAvailableTasks(userId: number): Observable<Task[]> {
    return this.http.get<Task[]>(`${this.baseUrl}/available/${userId}`);
  }

  /**
   * Assign the given task IDs to the user.
   * Expects a JSON payload { userId, taskIds } and returns { isSuccess, message }.
   */
  assignTasks(userId: number, taskIds: number[]): Observable<{ isSuccess: boolean; message: string }> {
    return this.http.post<{ isSuccess: boolean; message: string }>(
      `${this.baseUrl}/assign/${userId}`,
      { userId, taskIds }
    );
  }
}
