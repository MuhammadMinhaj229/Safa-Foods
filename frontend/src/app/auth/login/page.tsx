"use client";

import React, { useState, useEffect } from 'react';
import { useRouter } from 'next/navigation';
import { useRole } from '@/lib/context/RoleContext';
import { authService } from '@/lib/services/auth.service';
import { SafaLogo } from '@/components/shared/SafaLogo';
import { Phone, Mail, Lock, ArrowRight, ShieldCheck, ChevronLeft, Fingerprint, Clock, AlertCircle } from 'lucide-react';
import Cookies from 'js-cookie';

export default function LoginPage() {
  const router = useRouter();
  const { setRole } = useRole();
  const [authMode, setAuthMode] = useState<'customer' | 'admin'>('customer');
  const [step, setStep] = useState<'input' | 'verify'>('input');
  
  // Form States
  const [identifier, setIdentifier] = useState('');
  const [otp, setOtp] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const [countdown, setCountdown] = useState(0);

  useEffect(() => {
    if (countdown > 0) {
      const timer = setTimeout(() => setCountdown(countdown - 1), 1000);
      return () => clearTimeout(timer);
    }
  }, [countdown]);

  const handleRequestOtp = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError('');
    try {
      await authService.requestCustomerOtp(identifier);
      setStep('verify');
      setCountdown(60);
    } catch (err: Error | unknown) {
      setError((err as Error).message || 'Failed to send OTP. Please try again.');
    } finally {
      setLoading(false);
    }
  };

  const handleVerifyOtp = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError('');
    try {
      const session = await authService.verifyCustomerOtp(identifier, otp);
      localStorage.setItem('safa_session', JSON.stringify(session));
      Cookies.set('safa_session', JSON.stringify(session), { expires: 7 });
      setRole('customer');
      router.push('/profile');
    } catch (err: Error | unknown) {
      setError((err as Error).message || 'Artisan verification failed.');
    } finally {
      setLoading(false);
    }
  };

  const handleAdminLogin = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError('');
    try {
      const session = await authService.adminLogin(email, password);
      localStorage.setItem('safa_session', JSON.stringify(session));
      Cookies.set('safa_session', JSON.stringify(session), { expires: 1 });
      setRole('admin');
      router.push('/admin');
    } catch (err: Error | unknown) {
      setError((err as Error).message || 'Invalid credentials.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="min-h-screen bg-[#F4F1EA] flex flex-col items-center justify-center p-6 relative overflow-hidden">
      {/* Heritage Backdrop Element */}
      <div className="absolute top-[-10%] left-[-10%] opacity-5 rotate-12 pointer-events-none scale-[3]">
        <SafaLogo className="w-96 h-96" color="#1E331B" />
      </div>

      <div className="w-full max-w-md relative z-10 animate-fade-up">
        {/* Branding Hub */}
        <div className="text-center mb-10">
          <SafaLogo className="w-16 h-16 mx-auto mb-6" color="#A68A56" />
          <h1 className="font-serif italic text-5xl text-[#1E331B]">Artisan Portal</h1>
          <p className="text-[10px] font-black uppercase tracking-[0.4em] text-[#A68A56] mt-4">Pure Identity Verification</p>
        </div>

        {/* Security Vault Card */}
        <div className="bg-white/70 backdrop-blur-3xl p-10 rounded-2xl shadow-[0_40px_100px_rgba(0,0,0,0.1)] border border-white">
          {/* Mode Selector Tab */}
          <div className="flex gap-4 p-1.5 bg-[#F4F1EA] rounded-full mb-10 border border-[#1E331B]/5">
            <button 
              onClick={() => { setAuthMode('customer'); setStep('input'); setError(''); }}
              className={`flex-1 py-3 text-[10px] font-black uppercase tracking-widest rounded-full transition-all ${authMode === 'customer' ? 'bg-[#1E331B] text-white shadow-lg' : 'text-[#1E331B]/40 hover:text-[#1E331B]'}`}
            >
              Artisan OTP
            </button>
            <button 
              onClick={() => { setAuthMode('admin'); setStep('input'); setError(''); }}
              className={`flex-1 py-3 text-[10px] font-black uppercase tracking-widest rounded-full transition-all ${authMode === 'admin' ? 'bg-[#1E331B] text-white shadow-lg' : 'text-[#1E331B]/40 hover:text-[#1E331B]'}`}
            >
              Partner Key
            </button>
          </div>

          {error && <div className="mb-6 p-4 bg-red-50 text-red-600 text-[10px] font-black uppercase tracking-widest text-center rounded-xl border border-red-100">{error}</div>}

          {/* ARTISAN FLOW (Flexible Identifier) */}
          {authMode === 'customer' && (
            <form onSubmit={step === 'input' ? handleRequestOtp : handleVerifyOtp} className="space-y-8">
              {step === 'input' ? (
                <div className="space-y-2">
                  <div className="flex justify-between items-end mb-2">
                    <label className="text-[10px] font-black uppercase tracking-[0.2em] text-[#6B705C] ml-1">Identity Identifier</label>
                    <span className="text-[9px] text-[#A68A56] font-bold uppercase tracking-widest">Mobile or Email</span>
                  </div>
                  <div className="relative group">
                    <Fingerprint className="absolute left-5 top-1/2 -translate-y-1/2 text-[#A68A56] group-focus-within:scale-110 transition-transform" size={20} />
                    <input 
                      type="text" 
                      placeholder="e.g. 9876543210 or name@domain.com"
                      value={identifier}
                      onChange={(e) => setIdentifier(e.target.value)}
                      required
                      className="w-full bg-[#F4F1EA]/50 border-none rounded-xl py-5 pl-14 pr-6 focus:ring-2 focus:ring-[#A68A56] transition-all font-medium text-lg placeholder:opacity-30"
                    />
                  </div>
                </div>
              ) : (
                <div className="space-y-6 animate-fade-up">
                  <button onClick={() => setStep('input')} className="flex items-center gap-2 text-[10px] font-black uppercase tracking-widest text-[#A68A56] mb-4 hover:translate-x-[-4px] transition-transform">
                    <ChevronLeft size={16} /> Edit Identity
                  </button>
                  <div className="space-y-2">
                    <div className="flex justify-between items-end mb-2">
                      <label className="text-[10px] font-black uppercase tracking-[0.2em] text-[#6B705C] ml-1">Verification Code</label>
                      {countdown > 0 ? (
                        <span className="text-[9px] text-[#A68A56] font-bold flex items-center gap-1 uppercase tracking-widest"><Clock size={10} /> {countdown}s</span>
                      ) : (
                        <button type="button" onClick={handleRequestOtp} className="text-[9px] text-[#1E331B] font-black uppercase tracking-widest border-b border-[#1E331B]">Resend OTP</button>
                      )}
                    </div>
                    <div className="relative group">
                      <ShieldCheck className="absolute left-5 top-1/2 -translate-y-1/2 text-[#A68A56]" size={20} />
                      <input 
                        type="text" 
                        placeholder="••••••"
                        value={otp}
                        onChange={(e) => setOtp(e.target.value)}
                        maxLength={6}
                        required
                        className="w-full bg-[#F4F1EA]/50 border-none rounded-xl py-6 pl-14 pr-6 focus:ring-2 focus:ring-[#A68A56] transition-all font-bold text-3xl tracking-[0.8em] text-center"
                      />
                    </div>
                  </div>
                </div>
              )}

              <button 
                type="submit" 
                disabled={loading}
                className="w-full btn-primary py-6 text-[11px] font-black uppercase tracking-[0.3em] flex items-center justify-center gap-4 group disabled:opacity-50"
              >
                {loading ? 'Processing Artisan Key...' : step === 'input' ? 'Dispatch Security OTP' : 'Unlock Dashboard'}
                <ArrowRight className="group-hover:translate-x-2 transition-transform" size={18} />
              </button>

              <div className="pt-4 text-center">
                 <p className="text-[9px] font-medium text-[#6B705C] tracking-[0.1em] opacity-60 leading-relaxed uppercase">
                    By verifying, you confirm access to your <br/> pure artisan shopping history.
                 </p>
              </div>
            </form>
          )}

          {/* PARTNER FLOW (Console) */}
          {authMode === 'admin' && (
            <form onSubmit={handleAdminLogin} className="space-y-8 animate-fade-up">
              <div className="space-y-2">
                <label className="text-[10px] font-black uppercase tracking-[0.2em] text-[#6B705C] ml-1">Operations Identity</label>
                <div className="relative group">
                  <Mail className="absolute left-5 top-1/2 -translate-y-1/2 text-[#A68A56]" size={18} />
                  <input 
                    type="email" 
                    placeholder="partner@safafoods.in"
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                    required
                    className="w-full bg-[#F4F1EA]/50 border-none rounded-xl py-5 pl-14 pr-6 focus:ring-2 focus:ring-[#A68A56] transition-all font-medium placeholder:opacity-30"
                  />
                </div>
              </div>

              <div className="space-y-2">
                <label className="text-[10px] font-black uppercase tracking-[0.2em] text-[#6B705C] ml-1">Secure Authorization Key</label>
                <div className="relative group">
                  <Lock className="absolute left-5 top-1/2 -translate-y-1/2 text-[#A68A56]" size={18} />
                  <input 
                    type="password" 
                    placeholder="••••••••"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                    required
                    className="w-full bg-[#F4F1EA]/50 border-none rounded-xl py-5 pl-14 pr-6 focus:ring-2 focus:ring-[#A68A56] transition-all font-medium placeholder:opacity-30"
                  />
                </div>
              </div>

              <button 
                type="submit" 
                disabled={loading}
                className="w-full btn-primary py-6 text-[11px] font-black uppercase tracking-[0.3em] flex items-center justify-center gap-4 group"
              >
                {loading ? 'Decrypting Access...' : 'Enter Operations Cloud'}
                <ArrowRight className="group-hover:translate-x-2 transition-transform" size={18} />
              </button>

              <div className="text-center">
                 <a href="#" className="text-[9px] font-black uppercase tracking-[0.2em] text-[#A68A56] hover:text-[#1E331B] transition-colors">Credential Restoration?</a>
              </div>
            </form>
          )}
        </div>
      </div>
    </div>
  );
}
