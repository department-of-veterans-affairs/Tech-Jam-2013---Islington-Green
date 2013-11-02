namespace IG.Analyitcs

open System

type DrugEstimator() = 
    member this.GetDateDrugsAreEmpty (fillDate: System.DateTime, daysSupply : int) =
                                 let timeSpan = TimeSpan.FromDays((float)daysSupply)
                                 fillDate + timeSpan
type WeightComparer() =
    member this.GetCohort (weightList: int[])= 
                            weightList
                            |> Seq.map(fun i -> float(i))
                            |> Seq.average

    member this.GetDifferenceToCohort (baseWeight: int, cohortAverage: float)=
                            float(baseWeight) - cohortAverage


