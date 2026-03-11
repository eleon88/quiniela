import { TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { RoundDetailComponent } from './round-detail';

describe('RoundDetailComponent', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RoundDetailComponent],
      providers: [provideRouter([])],
    }).compileComponents();
  });

  it('should create', () => {
    const fixture = TestBed.createComponent(RoundDetailComponent);
    fixture.componentRef.setInput('boardId', 'board-1');
    fixture.componentRef.setInput('roundId', 'round-1');
    expect(fixture.componentInstance).toBeTruthy();
  });
});
