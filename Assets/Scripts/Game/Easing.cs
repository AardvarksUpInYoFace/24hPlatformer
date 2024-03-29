﻿using UnityEngine;
using System.Collections;

namespace JoniUtility
{
    /* 
     * Functions taken from Tween.js - Licensed under the MIT license
     * at https://github.com/sole/tween.js
     */
    public class Easing
    {
        public enum Direction { In, Out, InOut };
        public enum Function { Quadratic, Cubic, Quartic, Quintic, Sinusoidal, Exponential, Circular, Elastic, Back, Bounce };

        /*
        protected IEnumerator CoFadeImageAlpha(float iterator, float time, float startA, float endA, Image image)
        {
            while (iterator < time)
            {
                iterator += Time.deltaTime;
                if (iterator > time) iterator = time;

                float val = JoniUtility.Easing.GetTerpedPosition(iterator, time, JoniUtility.Easing.Function.Sinusoidal, JoniUtility.Easing.Direction.Out);

                float newA = Mathf.Lerp(startA, endA, val);

                Color col = image.color;

                image.color = new Color(col.r, col.g, col.b, newA);
                yield return 0;
            }
        }
        */


        public static float GetTerpedPosition(float time, float totalTime, Function function, Direction direction)
        {

            switch (function)
            {
                case Function.Quadratic:
                    switch (direction)
                    {
                        case Direction.In:
                            return Quadratic.In(time / totalTime);
                        case Direction.Out:
                            return Quadratic.Out(time / totalTime);
                        default:
                            return Quadratic.InOut(time / totalTime);
                    }
                case Function.Cubic:
                    switch (direction)
                    {
                        case Direction.In:
                            return Cubic.In(time / totalTime);
                        case Direction.Out:
                            return Cubic.Out(time / totalTime);
                        default:
                            return Cubic.InOut(time / totalTime);
                    }
                case Function.Quartic:
                    switch (direction)
                    {
                        case Direction.In:
                            return Quartic.In(time / totalTime);
                        case Direction.Out:
                            return Quartic.Out(time / totalTime);
                        default:
                            return Quartic.InOut(time / totalTime);
                    }
                case Function.Quintic:
                    switch (direction)
                    {
                        case Direction.In:
                            return Quintic.In(time / totalTime);
                        case Direction.Out:
                            return Quintic.Out(time / totalTime);
                        default:
                            return Quintic.InOut(time / totalTime);
                    }
                case Function.Sinusoidal:
                    switch (direction)
                    {
                        case Direction.In:
                            return Sinusoidal.In(time / totalTime);
                        case Direction.Out:
                            return Sinusoidal.Out(time / totalTime);
                        default:
                            return Sinusoidal.InOut(time / totalTime);
                    }
                case Function.Exponential:
                    switch (direction)
                    {
                        case Direction.In:
                            return Exponential.In(time / totalTime);
                        case Direction.Out:
                            return Exponential.Out(time / totalTime);
                        default:
                            return Exponential.InOut(time / totalTime);
                    }
                case Function.Circular:
                    switch (direction)
                    {
                        case Direction.In:
                            return Circular.In(time / totalTime);
                        case Direction.Out:
                            return Circular.Out(time / totalTime);
                        default:
                            return Circular.InOut(time / totalTime);
                    }
                case Function.Elastic:
                    switch (direction)
                    {
                        case Direction.In:
                            return Elastic.In(time / totalTime);
                        case Direction.Out:
                            return Elastic.Out(time / totalTime);
                        default:
                            return Elastic.InOut(time / totalTime);
                    }
                case Function.Back:
                    switch (direction)
                    {
                        case Direction.In:
                            return Back.In(time / totalTime);
                        case Direction.Out:
                            return Back.Out(time / totalTime);
                        default:
                            return Back.InOut(time / totalTime);
                    }
                default://Function.Bounce:
                    switch (direction)
                    {
                        case Direction.In:
                            return Bounce.In(time / totalTime);
                        case Direction.Out:
                            return Bounce.Out(time / totalTime);
                        default:
                            return Bounce.InOut(time / totalTime);
                    }
            }
        }

        public static float Linear(float k)
        {
            return k;
        }

        public class Quadratic
        {
            public static float In(float k)
            {
                return k * k;
            }

            public static float Out(float k)
            {
                return k * (2f - k);
            }

            public static float InOut(float k)
            {
                if ((k *= 2f) < 1f) return 0.5f * k * k;
                return -0.5f * ((k -= 1f) * (k - 2f) - 1f);
            }
        };

        public class Cubic
        {
            public static float In(float k)
            {
                return k * k * k;
            }

            public static float Out(float k)
            {
                return 1f + ((k -= 1f) * k * k);
            }

            public static float InOut(float k)
            {
                if ((k *= 2f) < 1f) return 0.5f * k * k * k;
                return 0.5f * ((k -= 2f) * k * k + 2f);
            }
        };

        public class Quartic
        {
            public static float In(float k)
            {
                return k * k * k * k;
            }

            public static float Out(float k)
            {
                return 1f - ((k -= 1f) * k * k * k);
            }

            public static float InOut(float k)
            {
                if ((k *= 2f) < 1f) return 0.5f * k * k * k * k;
                return -0.5f * ((k -= 2f) * k * k * k - 2f);
            }
        };

        public class Quintic
        {
            public static float In(float k)
            {
                return k * k * k * k * k;
            }

            public static float Out(float k)
            {
                return 1f + ((k -= 1f) * k * k * k * k);
            }

            public static float InOut(float k)
            {
                if ((k *= 2f) < 1f) return 0.5f * k * k * k * k * k;
                return 0.5f * ((k -= 2f) * k * k * k * k + 2f);
            }
        };

        public class Sinusoidal
        {
            public static float In(float k)
            {
                return 1f - Mathf.Cos(k * Mathf.PI / 2f);
            }

            public static float Out(float k)
            {
                return Mathf.Sin(k * Mathf.PI / 2f);
            }

            public static float InOut(float k)
            {
                return 0.5f * (1f - Mathf.Cos(Mathf.PI * k));
            }
        };

        public class Exponential
        {
            public static float In(float k)
            {
                return k == 0f ? 0f : Mathf.Pow(1024f, k - 1f);
            }

            public static float Out(float k)
            {
                return k == 1f ? 1f : 1f - Mathf.Pow(2f, -10f * k);
            }

            public static float InOut(float k)
            {
                if (k == 0f) return 0f;
                if (k == 1f) return 1f;
                if ((k *= 2f) < 1f) return 0.5f * Mathf.Pow(1024f, k - 1f);
                return 0.5f * (-Mathf.Pow(2f, -10f * (k - 1f)) + 2f);
            }
        };

        public class Circular
        {
            public static float In(float k)
            {
                return 1f - Mathf.Sqrt(1f - k * k);
            }

            public static float Out(float k)
            {
                return Mathf.Sqrt(1f - ((k -= 1f) * k));
            }

            public static float InOut(float k)
            {
                if ((k *= 2f) < 1f) return -0.5f * (Mathf.Sqrt(1f - k * k) - 1);
                return 0.5f * (Mathf.Sqrt(1f - (k -= 2f) * k) + 1f);
            }
        };

        public class Elastic
        {
            public static float In(float k)
            {
                if (k == 0) return 0;
                if (k == 1) return 1;
                return -Mathf.Pow(2f, 10f * (k -= 1f)) * Mathf.Sin((k - 0.1f) * (2f * Mathf.PI) / 0.4f);
            }

            public static float Out(float k)
            {
                if (k == 0) return 0;
                if (k == 1) return 1;
                return Mathf.Pow(2f, -10f * k) * Mathf.Sin((k - 0.1f) * (2f * Mathf.PI) / 0.4f) + 1f;
            }

            public static float InOut(float k)
            {
                if ((k *= 2f) < 1f) return -0.5f * Mathf.Pow(2f, 10f * (k -= 1f)) * Mathf.Sin((k - 0.1f) * (2f * Mathf.PI) / 0.4f);
                return Mathf.Pow(2f, -10f * (k -= 1f)) * Mathf.Sin((k - 0.1f) * (2f * Mathf.PI) / 0.4f) * 0.5f + 1f;
            }
        };

        public class Back
        {
            static float s = 1.70158f;
            static float s2 = 2.5949095f;

            public static float In(float k)
            {
                return k * k * ((s + 1f) * k - s);
            }

            public static float Out(float k)
            {
                return (k -= 1f) * k * ((s + 1f) * k + s) + 1f;
            }

            public static float InOut(float k)
            {
                if ((k *= 2f) < 1f) return 0.5f * (k * k * ((s2 + 1f) * k - s2));
                return 0.5f * ((k -= 2f) * k * ((s2 + 1f) * k + s2) + 2f);
            }
        };

        public class Bounce
        {
            public static float In(float k)
            {
                return 1f - Out(1f - k);
            }

            public static float Out(float k)
            {
                if (k < (1f / 2.75f))
                {
                    return 7.5625f * k * k;
                }
                else if (k < (2f / 2.75f))
                {
                    return 7.5625f * (k -= (1.5f / 2.75f)) * k + 0.75f;
                }
                else if (k < (2.5f / 2.75f))
                {
                    return 7.5625f * (k -= (2.25f / 2.75f)) * k + 0.9375f;
                }
                else
                {
                    return 7.5625f * (k -= (2.625f / 2.75f)) * k + 0.984375f;
                }
            }

            public static float InOut(float k)
            {
                if (k < 0.5f) return In(k * 2f) * 0.5f;
                return Out(k * 2f - 1f) * 0.5f + 0.5f;
            }
        };
    }

    //Remember to use this unclamped lerp when using functions that go outside the bounds of 0 and 1.
    public static class ExtendedLerp
    {
        public static float LerpWithoutClamp(float A, float B, float t)
        {
            return A + (B - A) * t;
        }
    }
}
