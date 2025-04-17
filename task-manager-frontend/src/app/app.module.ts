import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { ReactiveFormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { TaskAssignmentComponent } from './components/task-assignment/task-assignment.component';
import { UserListComponent } from './components/user-list/user-list.component';
import { AssignedTasksComponent } from './components/assigned-tasks/assigned-tasks.component';
import { AvailableTasksComponent } from './components/available-tasks/available-tasks.component';

@NgModule({
  declarations: [
    AppComponent,
    TaskAssignmentComponent,
    UserListComponent,
    AssignedTasksComponent,
    AvailableTasksComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    ReactiveFormsModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
