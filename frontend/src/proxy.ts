import { NextResponse, type NextRequest } from 'next/server';

/**
 * Professional Artisan Middleware
 * Performs sub-millisecond route protection for secure artisan areas.
 */
export function proxy(request: NextRequest) {
  const session = request.cookies.get('safa_session')?.value;
  const { pathname } = request.nextUrl;

  // 1. Protected Paths: Profile (Customer Only)
  if (pathname.startsWith('/profile') || pathname.startsWith('/checkout')) {
    if (!session) {
      const loginUrl = new URL('/auth/login', request.url);
      loginUrl.searchParams.set('redirect', pathname);
      return NextResponse.redirect(loginUrl);
    }
  }

  // 2. Protected Paths: Admin Operations (Admin Only)
  if (pathname.startsWith('/admin')) {
    if (!session) {
      return NextResponse.redirect(new URL('/auth/login', request.url));
    }
    
    try {
      const auth = JSON.parse(session);
      // Backend returns userId as Guid, Admins have a specific role in real systems
      // For now, we rely on the existence of the session; in Phase 9 we'll add JWT decoding.
    } catch {
      return NextResponse.redirect(new URL('/auth/login', request.url));
    }
  }

  return NextResponse.next();
}

// Configuration for focused interception
export const config = {
  matcher: ['/profile/:path*', '/checkout/:path*', '/admin/:path*'],
};
