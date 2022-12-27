namespace Vp.LightSequencing.Wpf.Shell.Model
{
    /// <summary>
    /// Try and get the length it will take to run the show inside visual pinball
    /// </summary>
    public static class SequenceHelper
    {
        public static int GetSequenceLength(Sequence sequence, int interval, int tail, int repeat, int pause)
        {
            var length = 0;
            switch (sequence)
            {
                case Sequence.None:
                case Sequence.SeqAllOn:
                case Sequence.SeqAllOff:                                
                    break;
                case Sequence.SeqBlinking: //tail isn't used
                    length = interval <= 10 ? 27 : interval;
                    return length * repeat + (pause * 2);
                case Sequence.SeqRandom:                
                case Sequence.SeqMiddleOutHorizOn:
                case Sequence.SeqMiddleOutHorizOff:
                case Sequence.SeqMiddleInHorizOn:
                case Sequence.SeqMiddleInHorizOff:
                    length = interval * 30;
                    break;
                case Sequence.SeqRightOn:
                case Sequence.SeqRightOff:
                case Sequence.SeqLeftOn:
                case Sequence.SeqLeftOff:
                case Sequence.SeqStripe1HorizOn:
                case Sequence.SeqStripe1HorizOff:
                case Sequence.SeqStripe2HorizOn:
                case Sequence.SeqStripe2HorizOff:
                case Sequence.SeqHatch1HorizOn:
                case Sequence.SeqHatch1HorizOff:
                case Sequence.SeqHatch2HorizOn:
                case Sequence.SeqHatch2HorizOff:
                case Sequence.SeqHatch1VertOn:
                case Sequence.SeqHatch1VertOff:
                case Sequence.SeqHatch2VertOn:
                case Sequence.SeqHatch2VertOff:
                    length = interval * 50;
                    break;
                case Sequence.SeqMiddleOutVertOn:
                case Sequence.SeqMiddleOutVertOff:
                case Sequence.SeqMiddleInVertOn:
                case Sequence.SeqMiddleInVertOff:
                    length = interval * 75;
                    break;
                case Sequence.SeqUpOn:
                case Sequence.SeqUpOff:
                case Sequence.SeqDownOn:
                case Sequence.SeqDownOff:
                case Sequence.SeqStripe1VertOn:
                case Sequence.SeqStripe1VertOff:
                case Sequence.SeqStripe2VertOn:
                case Sequence.SeqStripe2VertOff:
                case Sequence.SeqCircleOutOn:
                case Sequence.SeqCircleOutOff:
                case Sequence.SeqCircleInOn:
                case Sequence.SeqCircleInOff:
                    length = interval * 100;
                    break;
                case Sequence.SeqDiagUpRightOn:
                case Sequence.SeqDiagUpRightOff:
                case Sequence.SeqDiagUpLeftOn:
                case Sequence.SeqDiagUpLeftOff:
                case Sequence.SeqDiagDownRightOn:
                case Sequence.SeqDiagDownRightOff:
                case Sequence.SeqDiagDownLeftOn:
                case Sequence.SeqDiagDownLeftOff:
                    length = interval * 200;
                    break;
                //degrees
                case Sequence.SeqClockRightOn:
                case Sequence.SeqClockRightOff:
                case Sequence.SeqClockLeftOn:
                case Sequence.SeqClockLeftOff:
                    length = tail > 0 ? 360 + (tail) : 360;
                    return interval * length * repeat + pause;
                case Sequence.SeqScrewRightOn:
                case Sequence.SeqScrewRightOff:
                case Sequence.SeqScrewLeftOn:
                case Sequence.SeqScrewLeftOff:
                case Sequence.SeqRadarRightOn:
                case Sequence.SeqRadarRightOff:
                case Sequence.SeqRadarLeftOn:
                case Sequence.SeqRadarLeftOff:
                case Sequence.SeqWiperRightOn:
                case Sequence.SeqWiperRightOff:
                case Sequence.SeqWiperLeftOn:
                case Sequence.SeqWiperLeftOff:
                case Sequence.SeqFanLeftUpOn:
                case Sequence.SeqFanLeftUpOff:
                case Sequence.SeqFanLeftDownOn:
                case Sequence.SeqFanLeftDownOff:
                case Sequence.SeqFanRightUpOn:
                case Sequence.SeqFanRightUpOff:
                case Sequence.SeqFanRightDownOn:
                case Sequence.SeqFanRightDownOff:
                    length = tail > 0 ? 180 + (tail) : 180;
                    return interval * length * repeat + pause;
                case Sequence.SeqArcBottomLeftUpOn:
                case Sequence.SeqArcBottomLeftUpOff:
                case Sequence.SeqArcBottomLeftDownOn:
                case Sequence.SeqArcBottomLeftDownOff:
                case Sequence.SeqArcBottomRightUpOn:
                case Sequence.SeqArcBottomRightUpOff:
                case Sequence.SeqArcBottomRightDownOn:
                case Sequence.SeqArcBottomRightDownOff:
                case Sequence.SeqArcTopLeftUpOn:
                case Sequence.SeqArcTopLeftUpOff:
                case Sequence.SeqArcTopLeftDownOn:
                case Sequence.SeqArcTopLeftDownOff:
                case Sequence.SeqArcTopRightUpOn:
                case Sequence.SeqArcTopRightUpOff:
                case Sequence.SeqArcTopRightDownOn:
                case Sequence.SeqArcTopRightDownOff:
                    length = tail > 0 ? 90 + (tail) : 90;
                    return interval * length * repeat + pause;
                default:
                    break;
            }

            return length * tail / 100 + length * repeat + pause;
        }
    }
}
