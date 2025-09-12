import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AnfoAddComponent } from './anfo-add.component';

describe('AnfoAddComponent', () => {
  let component: AnfoAddComponent;
  let fixture: ComponentFixture<AnfoAddComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AnfoAddComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AnfoAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
