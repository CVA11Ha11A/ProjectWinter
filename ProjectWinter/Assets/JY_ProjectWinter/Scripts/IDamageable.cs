using UnityEngine;

// 데미지를 입을 수 있는 타입들이 공통적으로 가져야 하는 인터페이스
public interface IDamageable
{
    // 데미지를 입을 수 있는 타입들은 IDamageable을 상속하고 OnDamage 메서드를 반드시 구현해야 한다
    // OnDamage 메서드는 입력으로 데미지 크기(damage), 맞은 지점(hitPoint), 맞은 표면의 방향(hitNormal)을 받는다
    void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal);
}

// 콜라이더 사용 시 맞은 지점과 표면 계산법
// 상대방의 피격 위치와 피격 방향을 근삿값으로 계산
//Vector3 hitPoint = other.ClosestPoint(transform.position);
//Vector3 hitNormal = transform.position - other.transform.position;


// Ray 사용시 맞은 지점과 표면 계산법
// RaycastHit hit 이라면, hit.point와 hit.nomal을 사용하면 된다.
