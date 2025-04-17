import { Component, Input } from '@angular/core';
import { Task } from '../../models/task.model';

@Component({
  selector: 'app-assigned-tasks',
  templateUrl: './assigned-tasks.component.html',
  styleUrls: ['./assigned-tasks.component.css']
})
export class AssignedTasksComponent {
  @Input() tasks: Task[] = [];
}
