import { TestBed } from '@angular/core/testing';
import { AuthService } from '@auth0/auth0-angular';
import { of } from 'rxjs';
import { LoginComponent } from './login';

const authMock = {
  isAuthenticated$: of(false),
  user$: of(null),
  loginWithRedirect: vi.fn(),
  logout: vi.fn(),
};

describe('LoginComponent', () => {
  beforeEach(async () => {
    vi.clearAllMocks();
    await TestBed.configureTestingModule({
      imports: [LoginComponent],
      providers: [{ provide: AuthService, useValue: authMock }],
    }).compileComponents();
  });

  it('should create', () => {
    const fixture = TestBed.createComponent(LoginComponent);
    expect(fixture.componentInstance).toBeTruthy();
  });

  it('login() calls loginWithRedirect', () => {
    const fixture = TestBed.createComponent(LoginComponent);
    fixture.componentInstance.login();
    expect(authMock.loginWithRedirect).toHaveBeenCalled();
  });

  it('signup() calls loginWithRedirect with screen_hint signup', () => {
    const fixture = TestBed.createComponent(LoginComponent);
    fixture.componentInstance.signup();
    expect(authMock.loginWithRedirect).toHaveBeenCalledWith({
      authorizationParams: { screen_hint: 'signup' },
    });
  });

  it('logout() calls logout with returnTo', () => {
    const fixture = TestBed.createComponent(LoginComponent);
    fixture.componentInstance.logout();
    expect(authMock.logout).toHaveBeenCalledWith({
      logoutParams: { returnTo: window.location.origin },
    });
  });
});
