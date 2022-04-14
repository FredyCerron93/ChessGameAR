using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Threading.Tasks;

[RequireComponent(typeof(MaterialSetter))]
[RequireComponent(typeof(IObjectTweener))]
public abstract class Piece : MonoBehaviour
{
	[SerializeField] private MaterialSetter materialSetter;
	[SerializeField] private Transform transform;



	public Board board { protected get; set; }
	public Vector2Int occupiedSquare { get; set; }
	public TeamColor team { get; set; }
	public bool hasMoved { get; private set; }
	public List<Vector2Int> avaliableMoves;
	public Vector3 pos { get; set; }
	public Vector2Int pos2 { get; set; }
	public Vector2Int pos3 { get; set; }
	public Vector2Int pos4 { get; set; }

	public Vector2Int a1 { get; set; }
	public Vector2Int a2 { get; set; }
	public Vector2Int a3 { get; set; }
	public Vector2Int a4 { get; set; }
	public Vector2Int a5 { get; set; }
	public Vector2Int a6 { get; set; }
	public Vector2Int a7 { get; set; }
	public Vector2Int a8 { get; set; }



	private IObjectTweener tweener;

	public bool anim { get; set; }
	public bool ded { get; set; }
	public bool ded2 { get; set; }
	public bool at { get; set; }
	public bool isat { get; set; }
	public bool ish { get; set; }
	public bool isq { get; set; }

	public float turnSpeed = .01f;
	Quaternion rot;
	Vector3 direction;


	Transform transform2;


	public abstract List<Vector2Int> SelectAvaliableSquares();




	void Start()
	{
		pos = transform.position;
		pos2 = board.CalculateCoordsFromPosition(pos);
		pos3 = board.CalculateCoordsFromPosition(pos);


		a1 = new Vector2Int((int)1, (int)0);
		a2 = new Vector2Int((int)0, (int)1);
		a3 = new Vector2Int((int)-1, (int)0);
		a4 = new Vector2Int((int)0, (int)-1);
		a5 = new Vector2Int((int)1, (int)1);
		a6 = new Vector2Int((int)1, (int)-1);
		a7 = new Vector2Int((int)-1, (int)1);
		a8 = new Vector2Int((int)-1, (int)-1);
		//transform2 = transform;
	}

	void Update()
	{
		pos = transform.position;


		pos2 = board.CalculateCoordsFromPosition(pos);


		//if (pos3 != pos2)
		//{
		//	anim = true;
		//}
		//
		//else 
		//{
		//	anim = false;
		//
		//}

		//pos3 = new Vector2Int((int)targetPosition.x, (int)targetPosition.z); 

		//transform.rotation = Quaternion.Slerp(transform.rotation, rot, turnSpeed);


	}

	IEnumerator Waiter(Piece att)
	{
		//yield on a new YieldInstruction that waits for 5 seconds.



		yield return new WaitForSecondsRealtime(1);

		at = true;

		yield return new WaitForSecondsRealtime(1);

		att.ded = true;
		at = false;

		yield return new WaitForSecondsRealtime(1);

		att.ded2 = false;


	}

	private void Awake()
	{
		avaliableMoves = new List<Vector2Int>();
		tweener = GetComponent<IObjectTweener>();
		materialSetter = GetComponent<MaterialSetter>();
		hasMoved = false;
	}

	public void SetMaterial(Material selectedMaterial)
	{
		materialSetter.SetSingleMaterial(selectedMaterial);
	}

	public bool IsFromSameTeam(Piece piece)
	{
		return team == piece.team;
	}

	public bool CanMoveTo(Vector2Int coords)
	{
		return avaliableMoves.Contains(coords);
	}

	public virtual void MovePiece(Vector2Int coords)
	{
		Vector3 targetPosition = board.CalculatePositionFromCoords(coords);
		occupiedSquare = coords;



		//transform.LookAt(targetPosition);


		//var step = 3 * Time.deltaTime;
		//transform.rotation = Quaternion.RotateTowards(transform.rotation, target.rotation, step);

		//transform2.position = targetPosition;
		//direction = (transform2.position - transform.position).normalized;
		//rot = Quaternion.LookRotation(direction);

		//pos3 = new Vector2Int((int)targetPosition.x, (int)targetPosition.z);
		hasMoved = true;
		//tweener.MoveTo(transform, targetPosition);

	}


	protected void TryToAddMove(Vector2Int coords)
	{
		avaliableMoves.Add(coords);
	}

	public void SetData(Vector2Int coords, TeamColor team, Board board)
	{
		this.team = team;
		occupiedSquare = coords;
		this.board = board;
		transform.position = board.CalculatePositionFromCoords(coords);
	}

	public bool IsAttackingPieceOfType<T>() where T : Piece
	{
		foreach (var square in avaliableMoves)
		{
			if (board.GetPieceOnSquare(square) is T)
				return true;
		}
		return false;
	} 

	public virtual int GetPiece()
	{
		return 0;
	}

	public void ValidateCloser()
	{
		bool t = DetectIfCloser(pos3);



		if (t == true)
		{
			pos4 = pos2;
		}
		if (t == false)
		{
			GetCloser();
		}
	}

	public void GetCloser()
	{
		foreach (var square in avaliableMoves)
		{
			DetectCloser(square);
		}
		foreach (var square in avaliableMoves)
		{
			DetectCloser2(square);
		}
	}


	public async void Animation(Vector2Int coords, Piece att)
	{

		if (ish != true)
		{

			Vector3 targetPosition = board.CalculatePositionFromCoords(coords);

			//
			//Debug.Log(team);

			if (team == (TeamColor)1)
			{
				transform.LookAt(targetPosition);
			}

			if (team == (TeamColor)0)
			{
				Vector3 point = 2.0f * transform.position - targetPosition;
				transform.LookAt(point);
			}




			pos3 = coords;

			ValidateCloser();

			if (isat == true)
			{
				att.ded2 = true;

				Vector3 tar = board.CalculatePositionFromCoords(pos4);

				anim = true;

				tweener.MoveTo(transform, tar);


				while (pos != tar)
				{
					await Task.Yield();
				}

				anim = false;
				at = true;


				while (at == true)
				{
					await Task.Yield();
				}

				att.ded = true;

				while (att.ded == true)
				{
					await Task.Yield();
				}


				anim = true;



				tweener.MoveTo(transform, targetPosition);

				while (pos2 != coords)
				{
					await Task.Yield();
				}

				isat = false;
				anim = false;

				att.ded2 = false;

			}
			else
			{
				Vector3 targetP = board.CalculatePositionFromCoords(coords);

				//transform.LookAt(targetP);

				pos3 = coords;

				anim = true;
				tweener.MoveTo(transform, targetP);

				while (pos2 != coords)
				{
					await Task.Yield();
				}

				anim = false;
			}
		}

		else
		{
			Vector3 targetPos = board.CalculatePositionFromCoords(coords);

			transform.LookAt(targetPos);



			pos3 = coords;


			if (isat == true)
			{
				anim = true;
				att.ded2 = true;
				tweener.MoveTo(transform, targetPos);
				StartCoroutine(Waiter(att));

				while (ded2 == true)
				{
					await Task.Yield();
				}
				anim = false;
			}
			else
			{
				Vector3 targetP = board.CalculatePositionFromCoords(coords);

				transform.LookAt(targetP);

				pos3 = coords;

				anim = true;
				tweener.MoveTo(transform, targetP);

				while (pos2 != coords)
				{
					await Task.Yield();
				}

				anim = false;
			}






		}


	}

	public void Animation2(Vector2Int coords)
	{
		Vector3 targetPosition = board.CalculatePositionFromCoords(coords);

		//transform.LookAt(targetPosition);



		pos3 = new Vector2Int((int)targetPosition.x, (int)targetPosition.z);



		tweener.MoveTo(transform, targetPosition);


	}

	public void DetectCloser(Vector2Int coords)
	{

		if (coords == pos3 + a5)
		{
			pos4 = pos3 + a5;

		}
		else if (coords == pos3 + a6)
		{
			pos4 = pos3 + a6;

		}
		else if (coords == pos3 + a7)
		{
			pos4 = pos3 + a7;

		}
		else if (coords == pos3 + a8)
		{
			pos4 = pos3 + a8;

		}


	}


	public void DetectCloser2(Vector2Int coords)
	{

		if (coords == pos3 + a1)
		{
			pos4 = pos3 + a1;


		}
		else if (coords == pos3 + a2)
		{
			pos4 = pos3 + a2;

		}
		else if (coords == pos3 + a3)
		{
			pos4 = pos3 + a3;

		}
		else if (coords == pos3 + a4)
		{
			pos4 = pos3 + a4;

		}




	}


	public bool DetectIfCloser(Vector2Int coords)
	{

		if (coords == pos2 + a1)
		{
			return true;
		}
		else if (coords == pos2 + a2)
		{
			return true;

		}
		else if (coords == pos2 + a3)
		{
			return true;

		}
		else if (coords == pos2 + a4)
		{
			return true;

		}
		else if (coords == pos2 + a5)
		{
			return true;

		}
		else if (coords == pos2 + a6)
		{
			return true;

		}
		else if (coords == pos2 + a7)
		{
			return true;

		}
		else if (coords == pos2 + a8)
		{
			return true;

		}

		return false;
	}





}