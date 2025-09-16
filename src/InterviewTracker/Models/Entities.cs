using System.ComponentModel.DataAnnotations;

namespace InterviewTracker.Models;

public enum ApplicationStatus { Open, Rejected, Offer, Accepted }

public enum InterviewType
{
    Screen, HR, TechCoding, TechSystemDesign, TechArchitecture,
    PairProgramming, TakeHome, Managerial, Culture
}

public enum InterviewStage { Applied, Screening, Round1, Round2, Final, Offer }
