export function authenticate(role: string, target: string) {
  if (role !== target) {
    return false;
  }
  return true;
}
