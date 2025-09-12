import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AnfoEditComponent } from './anfo-edit.component';

describe('AnfoEditComponent', () => {
  let component: AnfoEditComponent;
  let fixture: ComponentFixture<AnfoEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AnfoEditComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AnfoEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
