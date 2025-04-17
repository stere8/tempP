import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AvailableTasksComponent } from './available-tasks.component';

describe('AvailableTasksComponent', () => {
  let component: AvailableTasksComponent;
  let fixture: ComponentFixture<AvailableTasksComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AvailableTasksComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AvailableTasksComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
