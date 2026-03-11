import { TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { AuthService } from '@auth0/auth0-angular';
import { of } from 'rxjs';
import { LayoutComponent } from './layout';

const authMock = {
  isAuthenticated$: of(true),
  user$: of(null),
  loginWithRedirect: vi.fn(),
  logout: vi.fn(),
};

describe('LayoutComponent', () => {
  beforeEach(async () => {
    vi.clearAllMocks();
    await TestBed.configureTestingModule({
      imports: [LayoutComponent],
      providers: [
        provideRouter([]),
        { provide: AuthService, useValue: authMock },
      ],
    }).compileComponents();
  });

  it('should create', () => {
    const fixture = TestBed.createComponent(LayoutComponent);
    expect(fixture.componentInstance).toBeTruthy();
  });

  it('login() calls loginWithRedirect', () => {
    const fixture = TestBed.createComponent(LayoutComponent);
    fixture.componentInstance.login();
    expect(authMock.loginWithRedirect).toHaveBeenCalled();
  });

  it('logout() calls logout with returnTo', () => {
    const fixture = TestBed.createComponent(LayoutComponent);
    fixture.componentInstance.logout();
    expect(authMock.logout).toHaveBeenCalledWith({
      logoutParams: { returnTo: window.location.origin },
    });
  });

  it('isAuthenticated signal reflects true from mock', () => {
    const fixture = TestBed.createComponent(LayoutComponent);
    expect((fixture.componentInstance as any).isAuthenticated()).toBe(true);
  });
});
