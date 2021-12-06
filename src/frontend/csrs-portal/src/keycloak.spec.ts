import { Keycloak } from './keycloak';

describe('Keycloak', () => {
  it('should create an instance', () => {
    expect(new Keycloak()).toBeTruthy();
  });
});
