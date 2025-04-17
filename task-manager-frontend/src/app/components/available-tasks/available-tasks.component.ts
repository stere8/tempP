import { Component, Input, Output, EventEmitter } from '@angular/core';
import { Task } from '../../models/task.model';

@Component({
  selector: 'app-available-tasks',
  templateUrl: './available-tasks.component.html',
  styleUrls: ['./available-tasks.component.css']
})
export class AvailableTasksComponent {
  @Input() tasks: Task[] = [];
  @Output() assignTasks = new EventEmitter<number[]>();

  // Array to track selected task IDs
  selectedTaskIds: number[] = [];

  // Toggle selection on task click
  toggleSelection(task: Task): void {
    const index = this.selectedTaskIds.indexOf(task.id);
    if (index !== -1) {
      // Already selected: unselect it.
      this.selectedTaskIds.splice(index, 1);
    } else {
      // Not selected: add to selection.
      this.selectedTaskIds.push(task.id);
    }
  }

  // Emit the list of selected task IDs to assign them
  assignSelected(): void {
    if (this.selectedTaskIds.length > 0) {
      this.assignTasks.emit(this.selectedTaskIds);
      // Optionally clear the selection after assignment
      this.selectedTaskIds = [];
    }
  }
}
