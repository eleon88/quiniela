import { TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { BoardDetailComponent } from './board-detail';

describe('BoardDetailComponent', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BoardDetailComponent],
      providers: [provideRouter([])],
    }).compileComponents();
  });

  it('should create', () => {
    const fixture = TestBed.createComponent(BoardDetailComponent);
    fixture.componentRef.setInput('boardId', 'test-id');
    expect(fixture.componentInstance).toBeTruthy();
  });

  it('boardId() returns the set value', () => {
    const fixture = TestBed.createComponent(BoardDetailComponent);
    fixture.componentRef.setInput('boardId', 'abc-123');
    expect(fixture.componentInstance.boardId()).toBe('abc-123');
  });
});
