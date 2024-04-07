using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Dependencies.ChaserLib.Tools
{
    public class MathTools
    {
        //Same as Random.Range, but the returned value is between min and max, inclusive.
        public static int RandomRange(int min, int max) => min == max ? min : Random.Range(min, max + 1);

        public static int IntLength(int i)
        {
            if (i < 0)
                throw new ArgumentOutOfRangeException();

            if (i == 0)
                return 1;

            return (int) Math.Floor(Math.Log10(i)) + 1;
        }

        public static int HexToInt(char h)
        {
            if (h >= '0' && h <= '9')
                return h - '0';
            if (h >= 'a' && h <= 'f')
                return h - 'a' + 10;
            if (h >= 'A' && h <= 'F')
                return h - 'A' + 10;
            return -1;
        }

        public static char IntToHex(int n)
        {
            if (n <= 9)
                return (char) (n + 48);
            return (char) (n - 10 + 97);
        }

        public static int? Min(int? val1, int? val2)
        {
            if (val1 == null)
                return val2;
            return val2 == null ? val1 : Math.Min(val1.Value, val2.Value);
        }

        public static int? Max(int? val1, int? val2)
        {
            if (val1 == null)
                return val2;
            return val2 == null ? val1 : Math.Max(val1.Value, val2.Value);
        }

        public static double? Min(double? val1, double? val2)
        {
            if (val1 == null)
                return val2;
            return val2 == null ? val1 : Math.Min(val1.Value, val2.Value);
        }

        public static double? Max(double? val1, double? val2)
        {
            if (val1 == null)
                return val2;
            return val2 == null ? val1 : Math.Max(val1.Value, val2.Value);
        }

        public static bool ApproxEquals(double d1, double d2) => Math.Abs(d1 - d2) < Math.Abs(d1) * 1e-6;

        public static float ClampAngle360(float angle) => ClampAngle(angle, 360f);

        public static float ClampAngle90(float angle) => ClampAngle(angle, 90f);

        public static float ClampAngle180(float angle) => ClampAngle(angle, 180f);

        public static float ClampAngle(float angle, float limit)
        {
            while (angle > limit)
                angle -= limit;
            while (angle < 0f)
                angle += limit;
            return angle;
        }

        public static float NormalizeAngle180(float angle) => NormalizeAngle(angle, 180f);

        public static float NormalizeAngle360(float angle) => NormalizeAngle(angle, 360f);

        public static float NormalizeAngle(float angle, float limit)
        {
            while (angle < -limit)
                angle += 360f;

            while (angle > limit)
                angle -= 360f;

            return angle;
        }

        public static void PhysLookTowards(Rigidbody rBody, Vector3 up, Vector3 direction, float smoothTime,
            ref float lerpStep)
        {
            lerpStep += smoothTime * Time.fixedDeltaTime;
            var newRotation = GetLookRotationLerped(rBody.transform.rotation, up, direction, lerpStep);
            rBody.MoveRotation(newRotation);
        }

        public static Quaternion GetLookRotationLerped(Quaternion fromRotation, Vector3 up, Vector3 direction,
            float lerpStep)
        {
            if (direction == Vector3.zero)
                return fromRotation;

            var targetRotation = Quaternion.LookRotation(direction, up);
            return Quaternion.Slerp(fromRotation, targetRotation, lerpStep);
        }

        public static void LookTowards(Transform transf, Vector3 up, Vector3 direction, float smoothTime,
            ref float lerpStep)
        {
            lerpStep += smoothTime * Time.deltaTime;
            transf.rotation = GetLookRotationLerped(transf.rotation, up, direction, lerpStep);
        }

        public static int GetAngleDirection(Vector3 fwd, Vector3 targetDir, Vector3 up)
        {
            var perp = Vector3.Cross(fwd, targetDir);
            var dir = Vector3.Dot(perp, up);
            return dir > 0f ? 1 : dir < 0f ? -1 : 0;
        }

        public static float SpringLerp(float strength, float deltaTime)
        {
            if (deltaTime > 1f)
                deltaTime = 1f;
            var ms = Mathf.RoundToInt(deltaTime * 1000f);
            deltaTime = 0.001f * strength;
            var cumulative = 0f;
            for (var i = 0; i < ms; ++i)
                cumulative = Mathf.Lerp(cumulative, 1f, deltaTime);
            return cumulative;
        }

        public static float SpringLerp(float from, float to, float strength, float deltaTime)
        {
            if (deltaTime > 1f)
                deltaTime = 1f;
            var ms = Mathf.RoundToInt(deltaTime * 1000f);
            deltaTime = 0.001f * strength;
            for (var i = 0; i < ms; ++i)
                @from = Mathf.Lerp(@from, to, deltaTime);
            return @from;
        }

        public static Vector2 SpringLerp(Vector2 from, Vector2 to, float strength, float deltaTime) =>
            Vector2.Lerp(@from, to, SpringLerp(strength, deltaTime));

        public static Vector3 SpringLerp(Vector3 from, Vector3 to, float strength, float deltaTime) =>
            Vector3.Lerp(@from, to, SpringLerp(strength, deltaTime));

        public static Quaternion SpringLerp(Quaternion from, Quaternion to, float strength, float deltaTime) =>
            Quaternion.Slerp(@from, to, SpringLerp(strength, deltaTime));

        public static Rect ConvertToTexCoords(Rect rect, int width, int height)
        {
            var final = rect;
            if (!Mathf.Approximately(width, 0f) && !Mathf.Approximately(height, 0f))
            {
                final.xMin = rect.xMin / width;
                final.xMax = rect.xMax / width;
                final.yMin = 1f - rect.yMax / height;
                final.yMax = 1f - rect.yMin / height;
            }

            return final;
        }
    }
}