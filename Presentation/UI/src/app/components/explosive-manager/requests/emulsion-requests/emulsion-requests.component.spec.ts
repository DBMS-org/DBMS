import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EmulsionRequestsComponent } from './emulsion-requests.component';

describe('EmulsionRequestsComponent', () => {
  let component: EmulsionRequestsComponent;
  let fixture: ComponentFixture<EmulsionRequestsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EmulsionRequestsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EmulsionRequestsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
