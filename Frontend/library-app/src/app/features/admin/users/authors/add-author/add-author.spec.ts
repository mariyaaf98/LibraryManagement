import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AuthorFormComponent } from './add-author';

describe('AddAuthor', () => {
  let component: AuthorFormComponent;
  let fixture: ComponentFixture<AuthorFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AuthorFormComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(AuthorFormComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
