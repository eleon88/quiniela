import { TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { PageNotFoundComponent } from './page-not-found';

describe('PageNotFoundComponent', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PageNotFoundComponent],
      providers: [provideRouter([])],
    }).compileComponents();
  });

  it('should create', () => {
    const fixture = TestBed.createComponent(PageNotFoundComponent);
    expect(fixture.componentInstance).toBeTruthy();
  });
});
