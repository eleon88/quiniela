import { authGuardFn } from '@auth0/auth0-angular';
import { routes } from './app.routes';

describe('App Routes', () => {
  const root = routes[0];

  it('should have 2 top-level routes', () => {
    expect(routes.length).toBe(2);
  });

  it('wildcard route is last', () => {
    expect(routes[1].path).toBe('**');
  });

  it('root has 4 child paths', () => {
    expect(root.children?.length).toBe(4);
  });

  it('root redirects to boards', () => {
    const redirect = root.children?.find(c => c.path === '');
    expect(redirect?.redirectTo).toBe('boards');
  });

  it('admin route has canActivate with authGuardFn', () => {
    const admin = root.children?.find(c => c.path === 'admin');
    expect(admin?.canActivate).toContain(authGuardFn);
  });
});
