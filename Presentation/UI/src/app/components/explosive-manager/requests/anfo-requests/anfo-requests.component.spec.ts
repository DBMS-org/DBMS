import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AnfoRequestsComponent } from './anfo-requests.component';

describe('AnfoRequestsComponent', () => {
  let component: AnfoRequestsComponent;
  let fixture: ComponentFixture<AnfoRequestsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AnfoRequestsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AnfoRequestsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
