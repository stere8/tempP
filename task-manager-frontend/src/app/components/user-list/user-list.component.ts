import { Component, Input, Output, EventEmitter } from '@angular/core';
import { User } from '../../models/user.model';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css']
})
export class UserListComponent {
  @Input() users: User[] = [];
  @Output() userSelected = new EventEmitter<User>();

  selectUser(user: User): void {
    this.userSelected.emit(user);
  }
}
