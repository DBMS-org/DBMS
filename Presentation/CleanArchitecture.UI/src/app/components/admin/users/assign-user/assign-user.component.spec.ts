import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AssignUserComponent } from './assign-user.component';

describe('AssignUserComponent', () => {
  let component: AssignUserComponent;
  let fixture: ComponentFixture<AssignUserComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AssignUserComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AssignUserComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
